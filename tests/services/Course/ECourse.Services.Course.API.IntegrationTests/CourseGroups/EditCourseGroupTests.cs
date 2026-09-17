using System.Net;
using System.Net.Http.Json;
using ECourse.Services.Course.API.Contracts.CourseGroups.Requests;
using ECourse.Services.Course.API.Contracts.CourseGroups.Responses;
using ECourse.Services.Course.API.IntegrationTests.Infrastructure;
using ECourse.Services.Course.Application.Features.CourseGroups.GetById;

namespace ECourse.Services.Course.API.IntegrationTests.CourseGroups;
public class EditCourseGroupTests : IClassFixture<CourseWebApplicationFactory>
{
    private readonly CourseWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly string _baseUrl = "/api/course-groups";
    public EditCourseGroupTests(CourseWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
    [Fact]
    public async Task EditCourseGroup_ShouldReturnNoContent_WhenCourseGroupIsEdited()
    {
        // Arrange
        var courseGroupId = Guid.NewGuid();
        var createRequest = new CreateCourseGroupRequest
        (
            Title: "Test Course Group",
            Slug: "test-course-group",
            Description: "This is a test course group.",
            IsActive: true,
            ParentId: null
        );
        // Create a course group first
        var createResponse = await _client.PostAsJsonAsync(_baseUrl, createRequest);
        createResponse.EnsureSuccessStatusCode();
        var createdCourseGroup = await createResponse.Content.ReadFromJsonAsync<CreateCourseGroupResponse>();
        // Act
        var editRequest = new UpdateCourseGroupRequest
        (
            Title: "Updated Test Course Group",
            Slug: "updated-test-course-group",
            Description: "This is an updated test course group.",
            IsActive: false,
            ParentId: null
        );
        var editResponse = await _client.PutAsJsonAsync($"{_baseUrl}/{createdCourseGroup.Id}", editRequest);

        //request for the updated course group to verify the changes
        var updatedResponse = await _client.GetAsync($"{_baseUrl}/{createdCourseGroup.Id}");
        var updatedCourseGroup = await updatedResponse.Content.ReadFromJsonAsync<CourseGroupDetailsResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, editResponse.StatusCode);
        Assert.NotNull(updatedCourseGroup);
        Assert.Equal(editRequest.Title, updatedCourseGroup.Title);
        Assert.Equal(editRequest.Slug, updatedCourseGroup.Slug);
        Assert.Equal(editRequest.Description, updatedCourseGroup.Description);
        Assert.Equal(editRequest.IsActive, updatedCourseGroup.IsActive);
    }
    [Fact]
    public async Task EditCourseGroup_ShouldReturnNotFound_WhenCourseGroupDoesNotExist()
    {
        // Arrange
        var nonExistentCourseGroupId = Guid.NewGuid();
        var editRequest = new UpdateCourseGroupRequest
        (
            Title: "Non-existent Course Group",
            Slug: "non-existent-course-group",
            Description: "This course group does not exist.",
            IsActive: true,
            ParentId: null
        );
        // Act
        var editResponse = await _client.PutAsJsonAsync($"{_baseUrl}/{nonExistentCourseGroupId}", editRequest);
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, editResponse.StatusCode);
    }
    [Fact]
    public async Task EditCourseGroup_WhenParentIdChanged_UpdateChildParentId()
    {
        // Arrange
        var ParentRequest = new CreateCourseGroupRequest
        (
            Title: "Test Course Group",
            Slug: "test-course-group",
            Description: "This is a test course group.",
            IsActive: true,
            ParentId: null
        );
        // Create a parent course group first
        var ParentResponse = await _client.PostAsJsonAsync(_baseUrl, ParentRequest);
        ParentResponse.EnsureSuccessStatusCode();
        var ParentCourserGroup = await ParentResponse.Content.ReadFromJsonAsync<CreateCourseGroupResponse>();

        // Create a new child course group
        var ChildRequest = new CreateCourseGroupRequest
        (
            Title: "New Parent Course Group",
            Slug: "new-parent-course-group",
            Description: "This is a new parent course group.",
            IsActive: true,
            ParentId: null
        );
        var childResponse = await _client.PostAsJsonAsync(_baseUrl, ChildRequest);
        childResponse.EnsureSuccessStatusCode();
        var childCourseGroup = await childResponse.Content.ReadFromJsonAsync<CreateCourseGroupResponse>();

        //we want update the parent of child (second course group)
        // Act
        var editChildRequest = new UpdateCourseGroupRequest
        (
            Title: ChildRequest.Title,
            Slug: ChildRequest.Slug,
            Description: ChildRequest.Description,
            IsActive: ChildRequest.IsActive,
            ParentId: ParentCourserGroup.Id // Change the parent ID
        );
        var editResponse = await _client.PutAsJsonAsync($"{_baseUrl}/{childCourseGroup.Id}", editChildRequest);
        //request for the updated course group to verify the changes
        var updatedResponse = await _client.GetAsync($"{_baseUrl}/{childCourseGroup.Id}");
        var updatedCourseGroup = await updatedResponse.Content.ReadFromJsonAsync<CourseGroupDetailsResponse>();
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, editResponse.StatusCode);
        Assert.NotNull(updatedCourseGroup);
        Assert.NotNull(updatedCourseGroup.ParentId);
        Assert.Equal(ParentCourserGroup.Id, updatedCourseGroup.ParentId);
    }

}

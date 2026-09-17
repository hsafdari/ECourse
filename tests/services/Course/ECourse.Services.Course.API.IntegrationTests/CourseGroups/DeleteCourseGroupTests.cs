using System.Net;
using System.Net.Http.Json;
using ECourse.Services.Course.API.Contracts.CourseGroups.Requests;
using ECourse.Services.Course.API.Contracts.CourseGroups.Responses;
using ECourse.Services.Course.API.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace ECourse.Services.Course.API.IntegrationTests.CourseGroups;
public class DeleteCourseGroupTests : IClassFixture<CourseWebApplicationFactory>
{
    private readonly CourseWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly string _baseUrl = "/api/course-groups";

    public DeleteCourseGroupTests(CourseWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }
    [Fact]
    public async Task DeleteCourseGroup_ShouldReturnNoContent_WhenCourseGroupIsDeleted()
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
        createdCourseGroup.Should().NotBeNull();
        // Act
        var deleteResponse = await _client.DeleteAsync($"{_baseUrl}/{createdCourseGroup.Id}");
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }
    [Fact]
    public async Task DeleteCourseGroup_ShouldReturnNotFound_WhenCourseGroupDoesNotExist()
    {
        // Arrange
        var nonExistentCourseGroupId = Guid.NewGuid();
        // Act
        var deleteResponse = await _client.DeleteAsync($"{_baseUrl}/{nonExistentCourseGroupId}");
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
    }
    [Fact]
    public async Task DeleteCourseGroup_ShouldReturnBadRequest_WhenCourseGroupHasChildren()
    {
        // Arrange
        var parentRequest = new CreateCourseGroupRequest
        (
            Title: "Parent Course Group",
            Slug: "parent-course-group",
            Description: "This is a parent course group.",
            IsActive: true,
            ParentId: null
        );
        var parentResponse = await _client.PostAsJsonAsync(_baseUrl, parentRequest);
        parentResponse.EnsureSuccessStatusCode();
        var parentCourseGroup = await parentResponse.Content.ReadFromJsonAsync<CreateCourseGroupResponse>();
        parentCourseGroup.Should().NotBeNull();
        var childRequest = new CreateCourseGroupRequest
        (
            Title: "Child Course Group",
            Slug: "child-course-group",
            Description: "This is a child course group.",
            IsActive: true,
            ParentId: parentCourseGroup.Id
        );
        var childResponse = await _client.PostAsJsonAsync(_baseUrl, childRequest);
        childResponse.EnsureSuccessStatusCode();
        var childCourseGroup = await childResponse.Content.ReadFromJsonAsync<CreateCourseGroupResponse>();
        childCourseGroup.Should().NotBeNull();
        // Act
        var deleteResponse = await _client.DeleteAsync($"{_baseUrl}/{parentCourseGroup.Id}");
        // Assert
        Assert.Equal(HttpStatusCode.Conflict, deleteResponse.StatusCode);
    }
    //[Fact]
    //public async Task DeleteCourseGroup_ShouldReturnBadRequest_WhenCourseGroupHasCourses()
    //{
    //    // Arrange
    //    var courseGroupRequest = new CreateCourseGroupRequest
    //    (
    //        Title: "Course Group with Courses",
    //        Slug: "course-group-with-courses",
    //        Description: "This is a course group with courses.",
    //        IsActive: true,
    //        ParentId: null
    //    );
    //    var courseGroupResponse = await _client.PostAsJsonAsync(_baseUrl, courseGroupRequest);
    //    courseGroupResponse.EnsureSuccessStatusCode();
    //    var courseGroup = await courseGroupResponse.Content.ReadFromJsonAsync<CreateCourseGroupResponse>();
    //    courseGroup.Should().NotBeNull();
    //    // Here you would typically create a course associated with the course group.
    //    // For this test, we assume that the course creation logic is handled elsewhere.
    //    // Act
    //    var deleteResponse = await _client.DeleteAsync($"{_baseUrl}/{courseGroup.Id}");
    //    // Assert
    //    Assert.Equal(HttpStatusCode.Conflict, deleteResponse.StatusCode);
    //}
}

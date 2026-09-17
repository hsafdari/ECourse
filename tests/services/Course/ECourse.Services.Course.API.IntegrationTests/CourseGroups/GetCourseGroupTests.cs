using System.Net;
using System.Net.Http.Json;
using ECourse.Services.Course.API.Contracts.CourseGroups.Requests;
using ECourse.Services.Course.API.Contracts.CourseGroups.Responses;
using ECourse.Services.Course.API.IntegrationTests.Infrastructure;
using ECourse.Services.Course.Application.Features.CourseGroups.GetById;

namespace ECourse.Services.Course.API.IntegrationTests.CourseGroups;
public class GetCourseGroupTests : IClassFixture<CourseWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private const string api_url = "/api/course-groups/";
    
    public GetCourseGroupTests(CourseWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
    }
    [Fact]
    public async Task GetById_WhenCourseGroupExists_ShouldReturnCourseGroupDetailsAsync()
    {
        // Arrange
        var createRequest = new CreateCourseGroupRequest(Title: "Mathematics", Slug: "mathematics", Description: "Math courses", IsActive: true, ParentId: null);
        var createResponse = await _httpClient.PostAsJsonAsync(api_url, createRequest);
        var createdCourseGroup = await createResponse.Content.ReadFromJsonAsync<CreateCourseGroupResponse>();
        // Act
        var getResponse = await _httpClient.GetAsync($"{api_url}{createdCourseGroup.Id}");
        var courseGroupDetails = await getResponse.Content.ReadFromJsonAsync<CourseGroupDetailsResponse>();
        // Assert
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        Assert.NotNull(courseGroupDetails);
        Assert.Equal(createdCourseGroup.Id, courseGroupDetails.Id);
        Assert.Equal(createRequest.Title, courseGroupDetails.Title);
        Assert.Equal(createRequest.Slug, courseGroupDetails.Slug);
        Assert.Equal(createRequest.Description, courseGroupDetails.Description);
    }
    [Fact]
    public async Task GetById_WhenCourseGroupDoesNotExist_ShouldReturnNotFoundAsync()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        // Act
        var getResponse = await _httpClient.GetAsync($"{api_url}{nonExistentId}");
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}

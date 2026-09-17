using System.Net;
using System.Net.Http.Json;
using ECourse.Services.Course.API.Contracts.CourseGroups.Requests;
using ECourse.Services.Course.API.IntegrationTests.Infrastructure;
using ECourse.Services.Course.Application.Features.CourseGroups.GetById;

namespace ECourse.Services.Course.API.IntegrationTests.CourseGroups
{
    public sealed class CreateCourseGroupTests : IClassFixture<CourseWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        private const string api_url = "/api/course-groups/";
        public CreateCourseGroupTests(CourseWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }
        [Fact]
        public async Task Create_WhenRequestIsValid_ShouldReturnGuidGroupId()
        {
            //Arrange           
            var request = new CreateCourseGroupRequest(Title: "hardware", Slug: "hardware", Description: "desc", IsActive: true, ParentId: null);
            //Act
            var Response = await _httpClient.PostAsJsonAsync(api_url, request);
            var courseGroupReturn = await Response.Content.ReadFromJsonAsync<CourseGroupDetailsResponse>();
            //Assert
            Assert.Equal(HttpStatusCode.Created, Response.StatusCode);
            Assert.NotNull(courseGroupReturn);
            Assert.NotEqual(Guid.Empty, courseGroupReturn.Id);

        }
        [Fact]
        public async Task Create_WhenRequestHasParentId_ShouldCreatedWithParentId()
        {
            //Arrange
            var parentRequest = new CreateCourseGroupRequest(Title: "IT", Slug: "It", Description: "desc", IsActive: true, ParentId: null);
            var parentResponse = await _httpClient.PostAsJsonAsync(api_url, parentRequest);

            Assert.Equal(HttpStatusCode.Created, parentResponse.StatusCode);

            var parent = await parentResponse.Content.ReadFromJsonAsync<CourseGroupDetailsResponse>();

            Assert.NotNull(parent);
            Assert.NotEqual(Guid.Empty, parent.Id);

            var childRequest = new CreateCourseGroupRequest(Title: "software", Slug: "software", Description: "desc", IsActive: true, ParentId: parent.Id);

            //Act
            var childResponse = await _httpClient.PostAsJsonAsync(api_url, childRequest);

            //Assert
            Assert.Equal(HttpStatusCode.Created, childResponse.StatusCode);
            var child =
                await childResponse.Content
                .ReadFromJsonAsync<CourseGroupDetailsResponse>();

            Assert.NotNull(child);
            Assert.NotEqual(Guid.Empty, child.Id);


            var getChildResponse = await _httpClient.GetAsync($"{api_url}{child.Id}");

            Assert.Equal(HttpStatusCode.OK, getChildResponse.StatusCode);

            var childDetails =
            await getChildResponse.Content
            .ReadFromJsonAsync<CourseGroupDetailsResponse>();

            Assert.NotNull(childDetails);
            Assert.Equal(parent.Id, childDetails.ParentId);

        }
        [Fact]
        public async Task Create_WhenRequestIsInvalid_ShouldReturnBadRequest()
        {
            //Arrange
            var request = new CreateCourseGroupRequest(Title: "", Slug: "", Description: "desc", IsActive: true, ParentId: null);
            //Act
            var Response = await _httpClient.PostAsJsonAsync(api_url, request);
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, Response.StatusCode);
        }
        [Fact]
        public async Task Create_WhenRequestHasNonExistentParentId_ShouldReturnNotFound()
        {
            //Arrange
            var request = new CreateCourseGroupRequest(Title: "software", Slug: "software", Description: "desc", IsActive: true, ParentId: Guid.NewGuid());
            //Act
            var Response = await _httpClient.PostAsJsonAsync(api_url, request);
            //Assert
            Assert.Equal(HttpStatusCode.NotFound, Response.StatusCode);
        }
        [Fact]
        public async Task Create_WhenRequestHasDuplicateSlug_ShouldReturnConflict()
        {
            //Arrange
            var request = new CreateCourseGroupRequest(Title: "test2", Slug: "test2", Description: "desc", IsActive: true, ParentId: null);
            //Act
            var Response1 = await _httpClient.PostAsJsonAsync(api_url, request);
            var Response2 = await _httpClient.PostAsJsonAsync(api_url, request);
            //Assert
            Assert.Equal(HttpStatusCode.Created, Response1.StatusCode);
            Assert.Equal(HttpStatusCode.Conflict, Response2.StatusCode);
        }
    }
}

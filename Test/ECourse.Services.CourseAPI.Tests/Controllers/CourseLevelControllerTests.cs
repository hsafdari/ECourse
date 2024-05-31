using ECourse.Services.CourseAPI.Models.Dto.CourseLevel;
using ECourse.Services.CourseAPI.Tests.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Infrastructure.Models;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text.Json;

namespace ECourse.Services.CourseAPI.Tests.Controllers
{
    public class CourseLevelControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        public CourseLevelControllerTests(WebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.CreateDefaultClient(new Uri("http://localhost/api/courselevel"));
        }
        [Fact]
        public async Task Get_ReturnSuccess()
        {
            var response = await _httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Get_ReturnsExpectedMediaType()
        {
            var response = await _httpClient.GetAsync("");
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }
        [Fact]
        public async Task Get_ReturnsContent()
        {
            var response = await _httpClient.GetAsync("");
            Assert.NotNull(response.Content);
            Assert.True(response.Content.Headers.ContentLength > 0);
        }
        [Fact]
        public async Task Should_Post_CourseLevel()
        {
            CourseLevelDto model = new CourseLevelDto();
            model=new CourseLevelData().FakeGetSingleRowDTO();
            var json=JsonSerializer.Serialize<CourseLevelDto>(model);
            //HttpContent httpContent = new HttpContent();
           
            //var response = await _httpClient.PostAsync("", json);
        }
       

    }
}

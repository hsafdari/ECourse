using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Models.Dto;
using ECourse.Services.CourseAPI.Tests.Data;
using ECourse.Services.CourseAPI.Tests.Utility;
using FluentAssertions;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Mongo2Go;
using MongoDB.Driver;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Text;
using System.Web;
using static Infrastructure.BaseDbContext;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static MongoDB.Driver.ReplaceOneResult;

namespace ECourse.Services.CourseAPI.Tests.Controllers
{
    public class CourseLevelControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _dbContext;
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly IMongoCollection<CourseLevel> _collection;
        private readonly string ApiUrl = "/api/CourseLevel";

        public CourseLevelControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
            using var scope = _factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _collection = _dbContext._database.GetCollection<CourseLevel>(CourseLevel.DocumentName);
        }
        [Fact]
        public async Task Get_CourseLevel_Returns_ResponseDto_AllItems()
        {
            //arrange
            //var client = 
            CourseLevelDataGenerator fakeDataGenerator = new CourseLevelDataGenerator();
            var itemsInserted = fakeDataGenerator.GetCourseLevels();
            _collection.InsertMany(itemsInserted);
            //Act
            var response = await _httpClient.GetAsync(ApiUrl);
            var responseContent = await response.Content.ReadAsStringAsync();
            var ResultCourseLeveApi = JsonConvert.DeserializeObject<ResponseDto>(responseContent);
            var ResultCourseLeveApiList = JsonConvert.DeserializeObject<List<CourseLevelDto>>(ResultCourseLeveApi.Result.ToString());
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var _count = _collection.Find(x => true).ToList().Count;         
            ResultCourseLeveApiList.Should().HaveCount(_count);
            ResultCourseLeveApiList.Should().NotBeNullOrEmpty();
            ResultCourseLeveApiList.Should().Contain(x => itemsInserted.Any(y => y.Title == x.Title));
            _collection.DeleteMany(x => itemsInserted.Any(y => y.Id == x.Id));

        }
        [Fact]
        public async Task Grid_CourseLevel_Returns_ResponseDto_FilterItems()
        {
            //arrange           
            CourseLevelDataGenerator fakeDataGenerator = new CourseLevelDataGenerator();
            GridQueryAdminDataGenerator gridQueryObj = new GridQueryAdminDataGenerator();
            var itemsInserted = fakeDataGenerator.GetCourseLevels(20);
            var gridQueryFakeData = gridQueryObj.GenerateFakeQuery();
            _collection.InsertMany(itemsInserted);
            var url = GridQueryAdmin.GridFilterUrl(ApiUrl + $"/Grid", gridQueryFakeData);
            //Act
            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            var ResultCourseLeveApi = JsonConvert.DeserializeObject<ResponseDto>(responseContent);            
            var ResultCourseLeveApiList = JsonConvert.DeserializeObject<List<CourseLevelDto>>(ResultCourseLeveApi.Result.ToString());
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var _count = _collection.Find(x => !x.IsDeleted).ToList().Count;
            ResultCourseLeveApi.Should().NotBeNull();
            ResultCourseLeveApiList.Should().NotBeNullOrEmpty();
            ResultCourseLeveApi.Count.Should().Be(_count);            
            _collection.DeleteMany(x => itemsInserted.Any(y => y.Id == x.Id));

        }
        [Fact]
        public async Task Get_CourseLevel_Returns_ResponseDto_OneItem()
        {
            //arrange       
            CourseLevelDataGenerator fakeDataGenerator = new CourseLevelDataGenerator();
            var itemInserted = fakeDataGenerator.GetCourseLevel();
            _collection.InsertOne(itemInserted);
            //act
            var responseOneItem = await _httpClient.GetAsync($"{ApiUrl}/{itemInserted.Id}");
            var responseContent = await responseOneItem.Content.ReadAsStringAsync();
            var ResultCourseLeveApi = JsonConvert.DeserializeObject<ResponseDto>(responseContent);
            var ResultCourseLevelApiOneItem = JsonConvert.DeserializeObject<CourseLevelDto>(ResultCourseLeveApi.Result.ToString());
            //Assert
            responseOneItem.StatusCode.Should().Be(HttpStatusCode.OK);
            responseOneItem.Should().NotBeNull();
            ResultCourseLevelApiOneItem.Should().NotBeNull();
            ResultCourseLevelApiOneItem.Title.Should().Be(itemInserted.Title);
            ResultCourseLevelApiOneItem.Title.Should().Be(_collection.Find(x => x.Id == itemInserted.Id).FirstOrDefault().Title);
            _collection.DeleteOne(x => x.Id == itemInserted.Id);
        }
        [Fact]
        public async Task Post_CourseLevel_Returns_ResponseDto_CourseLevelDto()
        {
            //arrange
            var itemInserted = new CourseLevelDataGenerator().GetCourseLevelDto();
            var content = new StringContent(JsonConvert.SerializeObject(itemInserted), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(ApiUrl, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var ResultCourseLeveApi = JsonConvert.DeserializeObject<ResponseDto>(responseContent);
            var ResultCourseLevelApiOneItem = JsonConvert.DeserializeObject<CourseLevelDto>(ResultCourseLeveApi.Result.ToString());
            //act
            var CourseLeveCollectionOneItem = _collection.Find(x => x.Id == ResultCourseLevelApiOneItem.Id).FirstOrDefault();
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().NotBeNull();
            ResultCourseLevelApiOneItem.Should().NotBeNull();
            CourseLeveCollectionOneItem.Should().NotBeNull();
            CourseLeveCollectionOneItem.Title.Should().Be(ResultCourseLevelApiOneItem.Title);
            _collection.DeleteOne(x => x.Id == ResultCourseLevelApiOneItem.Id);
        }
        [Fact]
        public async Task Post_CourseLevel_ReturnError_InvalidInput_ResponseDto_CourseLevelDto()
        {
            //arrange
            var itemInserted = new CourseLevelDataGenerator().GetCourseLevelDtoInvalidTitle();
            var content = new StringContent(JsonConvert.SerializeObject(itemInserted), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(ApiUrl, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var ResultCourseLeveApi = JsonConvert.DeserializeObject<ValidationProblemDetails>(responseContent);
            //act            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
            ResultCourseLeveApi.Should().NotBeNull();
            ResultCourseLeveApi.Errors.Should().ContainSingle("The length of 'Title' must be 200 characters or fewer");
        }
        [Fact]
        public async Task Put_CourseLevel_Returns_ResponseDto_CourseLevelDto()
        {
            //arrange
            var itemFakeInserted = new CourseLevelDataGenerator().GetCourseLevel();
            await _collection.InsertOneAsync(itemFakeInserted);
            CourseLevelDto courseLevelDto = new CourseLevelDto { 
                Id = itemFakeInserted.Id,
                Title=itemFakeInserted.Title+"-Edited",
                FileName=itemFakeInserted.FileName+"-Edited",
                FileLocation=itemFakeInserted.FileLocation,
                Icon=itemFakeInserted.Icon,
                IsDeleted=itemFakeInserted.IsDeleted                
            };
            var content = new StringContent(JsonConvert.SerializeObject(courseLevelDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(ApiUrl, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var ResultCourseLeveApi = JsonConvert.DeserializeObject<ResponseDto>(responseContent);
            var ResultCourseLevelApiOneItem = JsonConvert.DeserializeObject<Acknowledged>(ResultCourseLeveApi.Result.ToString());
            //act
            var CourseLeveCollectionOneItem = _collection.Find(x => x.Id == itemFakeInserted.Id).FirstOrDefault();
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().NotBeNull();
            ResultCourseLevelApiOneItem.Should().NotBeNull();
            ResultCourseLevelApiOneItem.IsAcknowledged.Should().BeTrue();

            CourseLeveCollectionOneItem.Should().NotBeNull();
            CourseLeveCollectionOneItem.Title.Should().Be(courseLevelDto.Title);
            CourseLeveCollectionOneItem.FileName.Should().Be(courseLevelDto.FileName);
            _collection.DeleteOne(x => x.Id == courseLevelDto.Id);
        }
        [Fact]
        public async Task DeleteOneItem_CourseLevel_Returns_ResponseDto_CourseLevelDto()
        {
            //arrange
            var itemFakeInserted = new CourseLevelDataGenerator().GetCourseLevel();
            await _collection.InsertOneAsync(itemFakeInserted);          
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{itemFakeInserted.Id}");

            var responseContent = await response.Content.ReadAsStringAsync();
            var ResultCourseLeveApi = JsonConvert.DeserializeObject<ResponseDto>(responseContent);
            var ResultCourseLevelApiOneItem = JsonConvert.DeserializeObject<Acknowledged>(ResultCourseLeveApi.Result.ToString());
            //act
            var CourseLeveCollectionOneItem = _collection.Find(x => x.Id == itemFakeInserted.Id).FirstOrDefault();
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().NotBeNull();
            ResultCourseLevelApiOneItem.Should().NotBeNull();
            ResultCourseLevelApiOneItem.IsAcknowledged.Should().BeTrue();

            CourseLeveCollectionOneItem.Should().NotBeNull();
            CourseLeveCollectionOneItem.IsDeleted.Should().BeTrue();
            _collection.DeleteOne(x => x.Id == itemFakeInserted.Id);
        }
        [Fact]
        public async Task DeleteManyItems_CourseLevel_Returns_ResponseDto_CourseLevelDto()
        {
            //arrange
            var itemsFakeInserted = new CourseLevelDataGenerator().GetCourseLevels();
            await _collection.InsertManyAsync(itemsFakeInserted);
             System.Collections.Specialized.NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            itemsFakeInserted.Select(x => x.Id).ToList().ForEach(x=>queryString.Add("ids",x));
                       
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/deletemany?{queryString}");

            var responseContent = await response.Content.ReadAsStringAsync();
            var ResultCourseLeveApi = JsonConvert.DeserializeObject<ResponseDto>(responseContent);
            var ResultCourseLevelApiOneItem = JsonConvert.DeserializeObject<Acknowledged>(ResultCourseLeveApi.Result.ToString());
            //act
            var CourseLeveCollectionItems = _collection.Find(x =>itemsFakeInserted.Any(y=>y.Id==x.Id)).ToList();
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().NotBeNull();
            ResultCourseLevelApiOneItem.Should().NotBeNull();
            ResultCourseLevelApiOneItem.IsAcknowledged.Should().BeTrue();

            CourseLeveCollectionItems.Should().NotBeNull();
            CourseLeveCollectionItems.Count.Should().NotBe(0);
            CourseLeveCollectionItems.Count.Should().Be(10);
            CourseLeveCollectionItems.Should().Contain(x => x.IsDeleted==true);
            _collection.DeleteMany(x => itemsFakeInserted.Any(y => y.Id == x.Id));
        }

    }
}

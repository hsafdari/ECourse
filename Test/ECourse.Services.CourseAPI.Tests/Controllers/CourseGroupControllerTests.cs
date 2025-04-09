using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Models.Dto;
using ECourse.Services.CourseAPI.Tests.Data;
using ECourse.Services.CourseAPI.Tests.Utility;
using FluentAssertions;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Web;
using static MongoDB.Driver.ReplaceOneResult;

namespace ECourse.Services.CourseAPI.Tests.Controllers
{
    public class CourseGroupControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _dbContext;
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly IMongoCollection<CourseGroup> _collection;
        private readonly string ApiUrl = "/api/CourseGroup";

        public CourseGroupControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
            using var scope = _factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _collection = _dbContext._database.GetCollection<CourseGroup>(CourseGroup.DocumentName);
        }
        [Fact]
        public async Task Get_CourseGroup_Returns_ResponseDto_AllItems()
        {
            //arrange
            //var client = 
            CourseGroupDataGenerator fakeDataGenerator = new CourseGroupDataGenerator();
            var itemsInserted = fakeDataGenerator.GetCourseGroups();
            _collection.InsertMany(itemsInserted);
            //Act
            var response = await _httpClient.GetAsync(ApiUrl);
            var responseContent = await response.Content.ReadAsStringAsync();
            var ResultCourseLeveApi = JsonConvert.DeserializeObject<ResponseDto>(responseContent);
            var ResultCourseLeveApiList = JsonConvert.DeserializeObject<List<CourseGroupDto>>(ResultCourseLeveApi.Result.ToString());
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var _count = _collection.Find(x => true).ToList().Count;         
            ResultCourseLeveApiList.Should().HaveCount(_count);
            ResultCourseLeveApiList.Should().NotBeNullOrEmpty();
            ResultCourseLeveApiList.Should().Contain(x => itemsInserted.Any(y => y.Title == x.Title));
            _collection.DeleteMany(x => itemsInserted.Any(y => y.Id == x.Id));

        }
        [Fact]
        public async Task Grid_CourseGroup_Returns_ResponseDto_FilterItems()
        {
            //arrange           
            CourseGroupDataGenerator fakeDataGenerator = new CourseGroupDataGenerator();
            GridQueryAdminDataGenerator gridQueryObj = new GridQueryAdminDataGenerator();
            var itemsInserted = fakeDataGenerator.GetCourseGroups(20);
            var gridQueryFakeData = gridQueryObj.GenerateFakeQuery();
            _collection.InsertMany(itemsInserted);
            var url = GridQueryAdmin.GridFilterUrl(ApiUrl + $"/Grid", gridQueryFakeData);
            //Act
            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            var ResultCourseLeveApi = JsonConvert.DeserializeObject<ResponseDto>(responseContent);            
            var ResultCourseLeveApiList = JsonConvert.DeserializeObject<List<CourseGroupDto>>(ResultCourseLeveApi.Result.ToString());
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var _count = _collection.Find(x => !x.IsDeleted).ToList().Count;
            ResultCourseLeveApi.Should().NotBeNull();
            ResultCourseLeveApiList.Should().NotBeNullOrEmpty();
            ResultCourseLeveApi.Count.Should().Be(_count);            
            _collection.DeleteMany(x => itemsInserted.Any(y => y.Id == x.Id));

        }
        [Fact]
        public async Task Get_CourseGroup_Returns_ResponseDto_OneItem()
        {
            //arrange       
            CourseGroupDataGenerator fakeDataGenerator = new CourseGroupDataGenerator();
            var itemInserted = fakeDataGenerator.GetCourseGroup();
            _collection.InsertOne(itemInserted);
            //act
            var responseOneItem = await _httpClient.GetAsync($"{ApiUrl}/{itemInserted.Id}");
            var responseContent = await responseOneItem.Content.ReadAsStringAsync();
            var ResultCourseLeveApi = JsonConvert.DeserializeObject<ResponseDto>(responseContent);
            var ResultCourseGroupApiOneItem = JsonConvert.DeserializeObject<CourseGroupDto>(ResultCourseLeveApi.Result.ToString());
            //Assert
            responseOneItem.StatusCode.Should().Be(HttpStatusCode.OK);
            responseOneItem.Should().NotBeNull();
            ResultCourseGroupApiOneItem.Should().NotBeNull();
            ResultCourseGroupApiOneItem.Title.Should().Be(itemInserted.Title);
            ResultCourseGroupApiOneItem.Title.Should().Be(_collection.Find(x => x.Id == itemInserted.Id).FirstOrDefault().Title);
            _collection.DeleteOne(x => x.Id == itemInserted.Id);
        }
        [Fact]
        public async Task Post_CourseGroup_Returns_ResponseDto_CourseGroupDto()
        {
            //arrange
            var itemInserted = new CourseGroupDataGenerator().GetCourseGroupDto();
            var content = new StringContent(JsonConvert.SerializeObject(itemInserted), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(ApiUrl, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var ResultCourseLeveApi = JsonConvert.DeserializeObject<ResponseDto>(responseContent);
            var ResultCourseGroupApiOneItem = JsonConvert.DeserializeObject<CourseGroupDto>(ResultCourseLeveApi.Result.ToString());
            //act
            var CourseLeveCollectionOneItem = _collection.Find(x => x.Id == ResultCourseGroupApiOneItem.Id).FirstOrDefault();
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().NotBeNull();
            ResultCourseGroupApiOneItem.Should().NotBeNull();
            CourseLeveCollectionOneItem.Should().NotBeNull();
            CourseLeveCollectionOneItem.Title.Should().Be(ResultCourseGroupApiOneItem.Title);
            _collection.DeleteOne(x => x.Id == ResultCourseGroupApiOneItem.Id);
        }
        [Fact]
        public async Task Post_CourseGroup_ReturnError_InvalidInput_ResponseDto_CourseGroupDto()
        {
            //arrange
            var itemInserted = new CourseGroupDataGenerator().GetCourseGroupDtoInvalidTitle();
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
        public async Task Put_CourseGroup_Returns_ResponseDto_CourseGroupDto()
        {
            //arrange
            var itemFakeInserted = new CourseGroupDataGenerator().GetCourseGroup();
            await _collection.InsertOneAsync(itemFakeInserted);
            CourseGroupDto CourseGroupDto = new CourseGroupDto {
                Title = itemFakeInserted.Title,
                Id = itemFakeInserted.Id,              
                IsDeleted=itemFakeInserted.IsDeleted                
            };
            var content = new StringContent(JsonConvert.SerializeObject(CourseGroupDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(ApiUrl, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var ResultCourseLeveApi = JsonConvert.DeserializeObject<ResponseDto>(responseContent);
            var ResultCourseGroupApiOneItem = JsonConvert.DeserializeObject<Acknowledged>(ResultCourseLeveApi.Result.ToString());
            //act
            var CourseLeveCollectionOneItem = _collection.Find(x => x.Id == itemFakeInserted.Id).FirstOrDefault();
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().NotBeNull();
            ResultCourseGroupApiOneItem.Should().NotBeNull();
            ResultCourseGroupApiOneItem.IsAcknowledged.Should().BeTrue();

            CourseLeveCollectionOneItem.Should().NotBeNull();
            CourseLeveCollectionOneItem.Title.Should().Be(CourseGroupDto.Title);            
            _collection.DeleteOne(x => x.Id == CourseGroupDto.Id);
        }
        [Fact]
        public async Task DeleteOneItem_CourseGroup_Returns_ResponseDto_CourseGroupDto()
        {
            //arrange
            var itemFakeInserted = new CourseGroupDataGenerator().GetCourseGroup();
            await _collection.InsertOneAsync(itemFakeInserted);          
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{itemFakeInserted.Id}");

            var responseContent = await response.Content.ReadAsStringAsync();
            var ResultCourseLeveApi = JsonConvert.DeserializeObject<ResponseDto>(responseContent);
            var ResultCourseGroupApiOneItem = JsonConvert.DeserializeObject<Acknowledged>(ResultCourseLeveApi.Result.ToString());
            //act
            var CourseLeveCollectionOneItem = _collection.Find(x => x.Id == itemFakeInserted.Id).FirstOrDefault();
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().NotBeNull();
            ResultCourseGroupApiOneItem.Should().NotBeNull();
            ResultCourseGroupApiOneItem.IsAcknowledged.Should().BeTrue();

            CourseLeveCollectionOneItem.Should().NotBeNull();
            CourseLeveCollectionOneItem.IsDeleted.Should().BeTrue();
            _collection.DeleteOne(x => x.Id == itemFakeInserted.Id);
        }
        [Fact]
        public async Task DeleteManyItems_CourseGroup_Returns_ResponseDto_CourseGroupDto()
        {
            //arrange
            var itemsFakeInserted = new CourseGroupDataGenerator().GetCourseGroups();
            await _collection.InsertManyAsync(itemsFakeInserted);
             System.Collections.Specialized.NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            itemsFakeInserted.Select(x => x.Id).ToList().ForEach(x=>queryString.Add("ids",x));
                       
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/deletemany?{queryString}");

            var responseContent = await response.Content.ReadAsStringAsync();
            var ResultCourseLeveApi = JsonConvert.DeserializeObject<ResponseDto>(responseContent);
            var ResultCourseGroupApiOneItem = JsonConvert.DeserializeObject<Acknowledged>(ResultCourseLeveApi.Result.ToString());
            //act
            var CourseLeveCollectionItems = _collection.Find(x =>itemsFakeInserted.Any(y=>y.Id==x.Id)).ToList();
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().NotBeNull();
            ResultCourseGroupApiOneItem.Should().NotBeNull();
            ResultCourseGroupApiOneItem.IsAcknowledged.Should().BeTrue();

            CourseLeveCollectionItems.Should().NotBeNull();
            CourseLeveCollectionItems.Count.Should().NotBe(0);
            CourseLeveCollectionItems.Count.Should().Be(10);
            CourseLeveCollectionItems.Should().Contain(x => x.IsDeleted==true);
            _collection.DeleteMany(x => itemsFakeInserted.Any(y => y.Id == x.Id));
        }

    }
}

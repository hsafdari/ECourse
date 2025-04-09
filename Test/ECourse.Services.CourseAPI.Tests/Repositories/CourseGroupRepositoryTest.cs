using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Repositories;
using ECourse.Services.CourseAPI.Tests.Data;
using FluentAssertions;
using Infrastructure.Utility;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using static Infrastructure.BaseDbContext;
namespace ECourse.Services.CourseAPI.Tests.Repositories
{
    public class CourseGroupRepositoryTest : IDisposable
    {
        protected readonly ICourseGroupRepository _repository;
        protected readonly IMongoCollection<CourseGroup> _collection;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly MongoDbRunner _runner;
        public CourseGroupRepositoryTest()
        {
            var _logger = new Mock<ILogger<CourseGroup>>().Object;
            ///initialize the in-memory database
            _runner = MongoDbRunner.Start();
            var client = new MongoClient(_runner.ConnectionString);
            var Database = client.GetDatabase("CourseDbTest");
            ///initialize the ApplicationDbContext
            ///
            IOptions<MongoDbSettings> settings = Options.Create(new MongoDbSettings() { ConnectionString = _runner.ConnectionString, DatabaseName = "CourseDbTest" });
            _applicationDbContext = new ApplicationDbContext(settings);
            // Initialize the repository with the mocked database
            _repository = new CourseGroupRepository(_applicationDbContext, _logger);
            _collection = Database.GetCollection<CourseGroup>(CourseGroup.DocumentName);
        }

        [Fact]
        public async Task Should_Create_Item()
        {
            //arrange           
            CourseGroupDataGenerator fakeDataGenerator = new CourseGroupDataGenerator();
            var item = fakeDataGenerator.GetCourseGroup();
            //act
            await _repository.AddAsync(item);
            //assert
            var result = _collection.Find(x => x.Title == item.Title).FirstOrDefault();

            result.Should().NotBeNull();
        }
        [Fact]
        public async Task Should_Get_Item()
        {
            //arrange
            CourseGroupDataGenerator fakeDataGenerator = new CourseGroupDataGenerator();
            var item = fakeDataGenerator.GetCourseGroup();
            await _collection.InsertOneAsync(item);
            //act
            var result = await _repository.GetByIdAsync(item.Id);
            //assert
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task Should_Get_All_Items()
        {
            //arrange
            CourseGroupDataGenerator fakeDataGenerator = new CourseGroupDataGenerator();
            var items = fakeDataGenerator.GetCourseGroups();
            await _collection.InsertManyAsync(items);
            //act
            var result = await _repository.GetAllAsync();
            //assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(items.Count());
        }
        [Fact]
        public async Task Should_Update_Item()
        {
            //arrange
            CourseGroupDataGenerator fakeDataGenerator = new CourseGroupDataGenerator();
            var item = fakeDataGenerator.GetCourseGroup();
            item.Id = ObjectId.GenerateNewId().ToString();
            await _collection.InsertOneAsync(item);
            item.Title = "Updated Title";
            //act
            await _repository.UpdateAsync(item);
            //assert
            var itemIsExist = _collection.Find(x => x.Title == item.Title).FirstOrDefault();
            itemIsExist.Should().NotBeNull();
            itemIsExist.Title.Should().Be(item.Title);
        }
        [Fact]
        public async Task Should_Delete_Item()
        {
            //arrange
            CourseGroupDataGenerator fakeDataGenerator = new CourseGroupDataGenerator();
            var item = fakeDataGenerator.GetCourseGroup();
            await _collection.InsertOneAsync(item);
            //act
            await _repository.DeleteAsync(item.Id);
            //assert
            var itemIsExist = _collection.Find(x => x.Id == item.Id).FirstOrDefault();
            itemIsExist.Should().BeNull();
        }
        [Fact]
        public async Task Should_Get_Grid()
        {
            //arrange
            CourseGroupDataGenerator fakeDataGenerator = new CourseGroupDataGenerator();
            var items = fakeDataGenerator.GetCourseGroups(100);
            await _collection.InsertManyAsync(items);
            //act
            GridQuery param = new GridQuery() { Filter = "", page = 1, top = 10, sortColumn = "Title", sortOrder = "asc" };
            var result = await _repository.GetGridDataAsync(param);
            //assert
            result.Item1.Should().NotBeNullOrEmpty();
            result.Item2.Should().Be(items.Count());
            result.Item1.Should().HaveCountLessThanOrEqualTo(param.top);
        }
        public void Dispose()
        {
            _runner.Dispose();
        }
    }
}
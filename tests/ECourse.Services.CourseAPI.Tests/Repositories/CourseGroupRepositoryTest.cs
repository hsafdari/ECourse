using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Repositories;
using ECourse.Services.CourseAPI.Tests.Data;
using FluentAssertions;
using Infrastructure.Tests.Utility;
using Infrastructure.Utility;
using MongoDB.Bson;
using MongoDB.Driver;
namespace ECourse.Services.CourseAPI.Tests.Repositories
{
    public class CourseGroupRepositoryTest : BaseRepositoryTestBase<CourseGroup>
    {
        protected readonly ICourseGroupRepository _repository;       
        public CourseGroupRepositoryTest():base(CourseGroup.DocumentName)
        {           
            ApplicationDbContext _applicationDbContext = new ApplicationDbContext(_options);
            _repository = new CourseGroupRepository(_applicationDbContext, Logger);
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
    }
}
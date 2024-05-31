using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Repositories;
using ECourse.Services.CourseAPI.Tests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Infrastructure.Utility;
using MongoDB.Bson;
using Moq;
using System.Collections;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace ECourse.Services.CourseAPI.Tests.Repositories
{
    public class CourseLevelRepositoryTest
    {
        private DbContextOptions<ApplicationDataContext> _dbContextOptions { get; set; }
        public Mock<IDbContextFactory<ApplicationDataContext>> contextFactoryMock { get; private set; }

        private readonly Mock<ICourseLevelRepository> _mockCourseLevelRepository;
        private CourseLevelRepository _courseLevelRepository;

        public CourseLevelRepositoryTest()
        {
            _mockCourseLevelRepository = new Mock<ICourseLevelRepository>();
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDataContext>().UseInMemoryDatabase("CourseLevelTestDb").Options;
            contextFactoryMock = new Mock<IDbContextFactory<ApplicationDataContext>>();
            contextFactoryMock.Setup(factory => factory.CreateDbContext()).Returns(new ApplicationDataContext(_dbContextOptions));
            _courseLevelRepository = new CourseLevelRepository(contextFactoryMock.Object);

        }
        public CourseLevel InsertFakeData()
        {
            using var context = new ApplicationDataContext(_dbContextOptions);
            var entity = new CourseLevelData().FakeGetSingleRow();
            if (context.CourseLevels.Count() > 0)
            {
                context.CourseLevels.ForEachAsync(x => { context.CourseLevels.Remove(x); });
            }
            context.CourseLevels.Add(entity);
            context.SaveChangesAsync();
            var result = context.CourseLevels.FirstOrDefault();
            return result;
        }
        public async Task<int> InsertFakeDataList()
        {
            var items = new List<CourseLevel>();
            var fakedata = new FakeDataGenerator().dataList();
            items = fakedata.Select(x => new CourseLevel
            {
                Id = ObjectId.Parse(x[0].ToString()),
                Title = x[1].ToString(),
                FileLocation = x[2].ToString(),
                Icon = x[3].ToString(),
                CreateDateTime = Convert.ToDateTime(x[4])

            }).ToList();
            using var context = new ApplicationDataContext(_dbContextOptions);
            if (context.CourseLevels.Count() > 0)
            {
                context.CourseLevels.ForEachAsync(x => { context.CourseLevels.Remove(x); });
            }
            context.CourseLevels.AddRange(items);
            context.SaveChanges(true);
            return context.CourseLevels.Count();
        }
        [Fact]
        public async Task Should_Return_Created_CourseLevel()
        {
            //Arrange            
            InsertFakeDataList();
            //ACT
            var result = _courseLevelRepository.GetMany(x => x.IsDeleted == false);
            _mockCourseLevelRepository.Setup(q => q.Create(It.IsAny<CourseLevel>()));
            //Assert
            Assert.NotNull(result.Result);
            Assert.Equal(5, result.Result.Count);
        }

        [Theory]
        [ClassData(typeof(FakeDataGenerator))]
        public async Task Should_Create_Rows_CourseLevel(ObjectId Id, string Title, string FileLocation, string Icon, DateTime CreateDateTime)
        {
            var item = new CourseLevel
            {
                Id = Id,
                Title = Title,
                FileLocation = FileLocation,
                Icon = Icon,
                CreateDateTime = CreateDateTime
            };
            _courseLevelRepository.Create(item);
            using var context = new ApplicationDataContext(_dbContextOptions);
            var result = context.CourseLevels.Where(x => x.Id == Id).FirstOrDefault();
            Assert.Equal(Title, result.Title);
        }
        [Fact]
        public async Task Should_Update_CourseLevel()
        {
            //Arrange
            var model = InsertFakeData();

            model.Title = "MyNewUpdate";
            model.FileName = "MyNewFileName";

            //Act
            _courseLevelRepository.Update(model);
            using var context = new ApplicationDataContext(_dbContextOptions);
            var newmodel = context.CourseLevels.FirstOrDefault();

            //Assert
            Assert.Equal(model.Title, newmodel.Title);
            Assert.Equal(model.FileName, newmodel.FileName);

        }
        [Fact]
        public async Task Should_Return_row_CourseLevel()
        {
            //Arrange   
            var model = InsertFakeData();
            //Act
            var result = _courseLevelRepository.GetById(x => x.Id == model.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(model.Title, result.Result.Title);
        }
        [Fact]
        public async Task Should_DeleteRow_CourseLevel()
        {
            var model = InsertFakeData();
            _courseLevelRepository.Delete(model);

            var context = new ApplicationDataContext(_dbContextOptions);
            var count = context.CourseLevels.Count();
            Assert.Equal(0, count);

        }
        [Theory]
        [ClassData(typeof(FakeGridDataGenerator))]
        public async Task Should_Return_Grid(string filter, int take, int skip, string orderby, string select)
        {
            //Arrange
            var query = new GridQuery
            {
                filter = filter,
                skip = skip,
                top = take,
                orderby = orderby,
                select = select
            };
            //Act
            InsertFakeDataList();
            var data=_courseLevelRepository.Grid(query);

            //Assert
            Assert.NotNull(data.Result);
            Assert.True(data.Result.Count > 0);
        }
    }
    /// <summary>
    /// Data Generated base CourseLevel class
    /// </summary>
    public class FakeDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] { MongoDB.Bson.ObjectId.GenerateNewId(), "Beginner", "/Uploads/Test/file.jpg", "/Uploads/test.jpg",DateTime.Now },
            new object[] { MongoDB.Bson.ObjectId.GenerateNewId(), "Beginner1", "/Uploads/Test/file1.jpg", "/Uploads/test1.jpg",DateTime.Now.AddHours(1) },
            new object[] { MongoDB.Bson.ObjectId.GenerateNewId(), "Beginner2", "/Uploads/Test/file2.jpg", "/Uploads/test2.jpg",DateTime.Now.AddHours(2) },
            new object[] { MongoDB.Bson.ObjectId.GenerateNewId(), "Beginner3", "/Uploads/Test/file3.jpg", "/Uploads/test3.jpg",DateTime.Now.AddHours(3) },
            new object[] { MongoDB.Bson.ObjectId.GenerateNewId(), "Beginner4", "/Uploads/Test/file4.jpg", "/Uploads/test4.jpg",DateTime.Now.AddHours(4) }
        };
        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public List<object[]> dataList()
        {
            return _data;
        }
    }
    /// <summary>
    /// Data Generated based on GridQuery class
    /// </summary>
    public class FakeGridDataGenerator : IEnumerable<object[]>
    {
        //GridQuery class properties
        //string? filter
        //int top
        //int skip
        //string? orderby
        //string? select
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] { "(Title == null ? \"\" : Title).ToLower().Contains(\"Beginner\".ToLower())", 10,0,"Id desc","" },
            new object[] { "(Title == null ? \"\" : Title).ToLower().Contains(\"1\".ToLower())",10,0,"Id desc","" },
            new object[] { "(Title == null ? \"\" : Title).ToLower().Contains(\"2\".ToLower())",10,0,"Id desc","" },
            new object[] { "(Title == null ? \"\" : Title).ToLower().Contains(\"3\".ToLower())",10,0,"Id desc","" },
            new object[] { "(Title == null ? \"\" : Title).ToLower().Contains(\"b\".ToLower())",10,0,"Id desc","" }
        };
        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

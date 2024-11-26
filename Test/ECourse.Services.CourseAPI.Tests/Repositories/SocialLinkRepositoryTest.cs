using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Repositories;
using ECourse.Services.CourseAPI.Tests.Data;
using Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Moq;

namespace ECourse.Services.CourseAPI.Tests.Repositories
{
    public class SocialLinkRepositoryTest : BaseRepositoryTest<SocialLink>
    {
        private readonly Mock<ISocialLinkRepository> _mockSocialLinkRepository;
        private SocialLinkRepository _socialLinkRepository;
        public SocialLinkRepositoryTest() : base("SocialLinkDb")
        {
            _mockSocialLinkRepository = new Mock<ISocialLinkRepository>();
            _socialLinkRepository = new SocialLinkRepository(contextFactoryMock.Object);
        }

        public override SocialLink InsertFakeData()
        {
            using var context = new ApplicationDataContext(_dbContextOptions);
            var entity = new SocialLinkData().FakeGetSingleRow();
            if (context.SocialLinks.Count() > 0)
            {
                context.SocialLinks.ForEachAsync(x => { context.SocialLinks.Remove(x); });
            }
            context.SocialLinks.Add(entity);
            context.SaveChangesAsync();
            var result = context.SocialLinks.FirstOrDefault();
            return result;
        }

        public override async Task<int> InsertFakeDataList()
        {
            var items = new List<SocialLink>();
            var fakedata = new FakeDataGenerator_Create_SocialLink().dataList();
            items = fakedata.Select(x => new SocialLink
            {
                Id = ObjectId.Parse(x[0].ToString()),
                Name = x[1].ToString(),
                FileName = x[2].ToString(),
                FileLocation = x[3].ToString(),
                Icon = x[4].ToString(),
                CreateDateTime = Convert.ToDateTime(x[5])

            }).ToList();
            using var context = new ApplicationDataContext(_dbContextOptions);
            if (context.SocialLinks.Count() > 0)
            {
                context.SocialLinks.ForEachAsync(x => { context.SocialLinks.Remove(x); });
            }
            context.SocialLinks.AddRange(items);
            context.SaveChanges(true);
            return context.SocialLinks.Count();
        }
        public override async Task Should_DeleteRow_Model()
        {
            var model = InsertFakeData();
            _socialLinkRepository.Delete(x => x.Id == model.Id);

            var context = new ApplicationDataContext(_dbContextOptions);
            var count = context.SocialLinks.Where(x => x.IsDeleted == false).Count();
            Assert.Equal(0, count);
        }

        public override async Task Should_Return_Created_Model()
        {
            //Arrange            
            InsertFakeDataList();
            //ACT
            var result = _socialLinkRepository.GetMany(x => x.IsDeleted == false);
            _mockSocialLinkRepository.Setup(q => q.Create(It.IsAny<SocialLink>()));
            //Assert
            Assert.NotNull(result.Result);
            Assert.Equal(5, result.Result.Count);
        }

        [Theory]
        [ClassData(typeof(FakeDataGenerator_Grid_SocialLink))]
        public override async Task Should_Return_Grid(string filter, int take, int skip, string orderby, string select)
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
            var data = _socialLinkRepository.Grid(query);

            //Assert
            Assert.NotNull(data.Result.Item1);
            Assert.True(data.Result.Item2 > 0);
        }
        [Fact]
        public override async Task Should_Return_Row_Model()
        {
            //Arrange   
            var model = InsertFakeData();
            //Act
            var result = _socialLinkRepository.GetById(x => x.Id == model.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(model.Name, result.Result.Name);
        }
        [Fact]
        public override async Task Should_Update_Model()
        {
            //Arrange
            var model = InsertFakeData();

            model.Name = "MyNewUpdate";
            model.FileName = "MyNewFileName";

            //Act
            _socialLinkRepository.Update(model);
            using var context = new ApplicationDataContext(_dbContextOptions);
            var newmodel = context.SocialLinks.FirstOrDefault();

            //Assert
            Assert.Equal(model.Name, newmodel.Name);
            Assert.Equal(model.FileName, newmodel.FileName);
        }
    }
    /// <summary>
    /// Data Generated base CourseLevel class
    /// </summary>
    public class FakeDataGenerator_Create_SocialLink : FakeDataGenerator
    {
        public FakeDataGenerator_Create_SocialLink() : base(new List<object[]>())
        {
            this._data = new List<object[]> {
  new object[] { MongoDB.Bson.ObjectId.GenerateNewId(), "Instagram", "Instagramlogo.png", "/Uploads/Test/file.jpg", "http://website.com/Uploads/test.jpg",DateTime.Now },
  new object[] { MongoDB.Bson.ObjectId.GenerateNewId(), "Facebook","facebooklogo.png", "/Uploads/Test/file1.jpg", "http://website.com/Uploads/test1.jpg",DateTime.Now.AddHours(1) },
  new object[] { MongoDB.Bson.ObjectId.GenerateNewId(), "Youtube", "Youtubelogo.png", "/Uploads/Test/file2.jpg", "http://website.com/Uploads/test2.jpg",DateTime.Now.AddHours(2) },
  new object[] { MongoDB.Bson.ObjectId.GenerateNewId(), "LnkedIn", "LnkedInlogo.png", "/Uploads/Test/file3.jpg", "http://website.com/Uploads/test3.jpg",DateTime.Now.AddHours(3) },
  new object[] { MongoDB.Bson.ObjectId.GenerateNewId(), "TikTok", "TikToklogo.png", "/Uploads/Test/file4.jpg", "http://website.com/Uploads/test4.jpg",DateTime.Now.AddHours(4) },
  };
        }
    }
    public class FakeDataGenerator_Grid_SocialLink : FakeDataGenerator
    {
        public FakeDataGenerator_Grid_SocialLink() : base(new List<object[]>())
        {
            this._data = new List<object[]> {
          new object[] { "(Name == null ? \"\" : Name).ToLower().Contains(\"Instagram\".ToLower())", 10,0,"Id desc","" },
        new object[] { "(Name == null ? \"\" : Name).ToLower().Contains(\"I\".ToLower())",10,0,"Id desc","" },
        new object[] { "(Name == null ? \"\" : Name).ToLower().Contains(\"a\".ToLower())", 10, 0, "Id desc", "" },
        new object[] { "(Name == null ? \"\" : Name).ToLower().Contains(\"s\".ToLower())", 10, 0, "Id desc", "" },
        new object[] { "(Name == null ? \"\" : Name).ToLower().Contains(\"t\".ToLower())", 10, 0, "Id desc", "" }
         };
        }
    }
}

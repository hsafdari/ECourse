using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Repositories;
using Infrastructure.Models;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ECourse.Services.CourseAPI.Tests.Repositories
{
    public abstract class BaseRepositoryTest<T>: BaseEntity
    {
        protected DbContextOptions<ApplicationDataContext> _dbContextOptions { get; set; }
        public required Mock<IDbContextFactory<ApplicationDataContext>> contextFactoryMock { get; set; }
        public BaseRepositoryTest(string DbName)
        {            
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDataContext>().UseInMemoryDatabase(DbName).Options;
            contextFactoryMock = new Mock<IDbContextFactory<ApplicationDataContext>>();
            contextFactoryMock.Setup(factory => factory.CreateDbContext()).Returns(new ApplicationDataContext(_dbContextOptions));
        }
        public abstract T InsertFakeData();

        public abstract Task<int> InsertFakeDataList();
        [Fact]
        public abstract Task Should_Return_Created_Model();
        [Fact]
        public abstract Task Should_Update_Model();
        [Fact]
        public abstract Task Should_Return_Row_Model();
        [Fact]
        public abstract Task Should_DeleteRow_Model();        
        public abstract Task Should_Return_Grid(string filter, int take, int skip, string orderby, string select);
    }
}

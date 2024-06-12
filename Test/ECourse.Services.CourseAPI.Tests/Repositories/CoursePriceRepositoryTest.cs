using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ECourse.Services.CourseAPI.Tests.Repositories
{
    public class CoursePriceRepositoryTest
    {
        private DbContextOptions<ApplicationDataContext> _dbContextOptions { get; set; }
        public Mock<IDbContextFactory<ApplicationDataContext>> contextFactoryMock { get; private set; }

        private readonly Mock<ICoursePriceRepository> _mockCoursePriceRepository;
        private CoursePriceRepository _coursePriceRepository;

        public CoursePriceRepositoryTest()
        {
            _mockCoursePriceRepository = new Mock<ICoursePriceRepository>();
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDataContext>().UseInMemoryDatabase("CoursePriceTestDb").Options;
            contextFactoryMock = new Mock<IDbContextFactory<ApplicationDataContext>>();
            contextFactoryMock.Setup(factory => factory.CreateDbContext()).Returns(new ApplicationDataContext(_dbContextOptions));
            _coursePriceRepository = new CoursePriceRepository(contextFactoryMock.Object);

        }
    }
}

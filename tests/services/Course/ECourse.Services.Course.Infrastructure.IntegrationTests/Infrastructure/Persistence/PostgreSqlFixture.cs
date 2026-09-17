using ECourse.Services.Course.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace ECourse.Services.Course.IntegrationTests.Infrastructure.Persistence
{
    public class PostgreSqlFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _container=
            new PostgreSqlBuilder("postgres:17-alpine")
            .WithDatabase("ecourse_course_tests")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
        public async Task DisposeAsync()
        {
            await _container.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            await _container.StartAsync();
            await using CourseDbContext dbContext = CreateDbContext();

            await dbContext.Database.MigrateAsync();
        }

        public CourseDbContext CreateDbContext()
        {
            DbContextOptions<CourseDbContext> options = 
                new DbContextOptionsBuilder<CourseDbContext>()
                .UseNpgsql(_container.GetConnectionString())
                .Options;
            return new CourseDbContext(options);
        }
    }
}

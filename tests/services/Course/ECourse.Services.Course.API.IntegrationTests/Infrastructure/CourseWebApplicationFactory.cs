using ECourse.Services.Course.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ECourse.Services.Course.API.IntegrationTests.Infrastructure
{
    public class CourseWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {       
        private readonly PostgreSqlFixture _database=new();             
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<CourseDbContext>>();
                services.AddDbContext<CourseDbContext>(option =>
                {
                    option.UseNpgsql(_database.ConnectionString);
                });
            });            

        }
        public async Task InitializeAsync()
        {
            // 1. Start PostgreSQL container
            await _database.InitializeAsync();

            // 2. Force WebApplicationFactory to build the application
            using var scope = Services.CreateScope();

            var dbContext =
                scope.ServiceProvider.GetRequiredService<CourseDbContext>();

            // 3. Create schema/tables
            await dbContext.Database.MigrateAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _database.DisposeAsync();
        }
    }
}

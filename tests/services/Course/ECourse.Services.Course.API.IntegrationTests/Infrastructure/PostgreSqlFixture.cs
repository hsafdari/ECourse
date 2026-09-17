using Testcontainers.PostgreSql;

namespace ECourse.Services.Course.API.IntegrationTests.Infrastructure
{
    public class PostgreSqlFixture : IAsyncLifetime
    {
        private PostgreSqlContainer _container { set; get; }
        public string ConnectionString =>
        _container.GetConnectionString();
        public PostgreSqlFixture()
        {
            _container = new PostgreSqlBuilder()
                .WithImage("postgres:17-alpine")
                .WithDatabase("course-datbase")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();
        }
        public async Task InitializeAsync()
        {
            await _container.StartAsync();
        }
        public async Task DisposeAsync()
        {
            await _container.DisposeAsync().AsTask();
        }
        
    }
}

using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;

namespace ECourse.Services.CourseAPI.Tests.Utility
{
    public class ECourseWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private readonly MongoDbRunner _runner;
        public ECourseWebApplicationFactory()
        {
            _runner = MongoDbRunner.Start();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // remove original Mongo setup
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ApplicationDbContext));
                if (descriptor != null) services.Remove(descriptor);

                //  Remove MongoDbSettings if registered
                var mongoSettingsDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IOptions<BaseDbContext.MongoDbSettings>));
                if (mongoSettingsDescriptor != null)
                {
                    services.Remove(mongoSettingsDescriptor);
                }
                // Register MongoDbSettings with Mongo2Go connection
                var testSettings = new BaseDbContext.MongoDbSettings
                {
                    ConnectionString = _runner.ConnectionString,
                    DatabaseName = "TestDb"
                };

                var options = Options.Create(testSettings);
                services.AddSingleton<IOptions<BaseDbContext.MongoDbSettings>>(options);

                // Register test ApplicationDbContext
                services.AddSingleton<ApplicationDbContext>();
            });
        }
    }
}

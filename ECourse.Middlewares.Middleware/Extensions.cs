using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Middleware
{
    public static class Extensions
    {
        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoOptions>(configuration.GetSection("mongo"));
            services.AddSingleton(c =>
            {
                var options = c.GetService<IOptions<MongoOptions>>();
                if (options != null)
                {
                    return new MongoClient(options.Value.ConnectionString);

                }
                throw new Exception("ConnectionString not found");
            });
            services.AddSingleton(c =>
            {
                var options = c.GetService<IOptions<MongoOptions>>();
                var client = c.GetService<MongoClient>();
                if (options != null && client!=null)
                {
                    return client.GetDatabase(options.Value.Database);
                }
                throw new Exception("Mongo Database not found");
            });
        }
    }
}

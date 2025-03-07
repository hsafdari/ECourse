using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure
{
    public class BaseDbContext
    {
        public IMongoDatabase _database { get; }
        public BaseDbContext(IOptions<MongoDbSettings> settings)
        {
            MongoClient mongoClient = new(settings.Value.ConnectionString);
            _database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        }
        public BaseDbContext(string connectionString, string databaseName)
        {
            MongoClient mongoClient = new(connectionString);
            _database = mongoClient.GetDatabase(databaseName);
        }
        public IMongoCollection<TModel> GetCollection<TModel>(string collectionName)
        {
            return _database.GetCollection<TModel>(collectionName);
        }
        public class MongoDbSettings
        {
            public string ConnectionString { get; set; } = null!;
            public string DatabaseName { get; set; } = null!;
        }
    }
}

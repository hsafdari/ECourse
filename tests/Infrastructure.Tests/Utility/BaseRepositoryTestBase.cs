using Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.BaseDbContext;

namespace Infrastructure.Tests.Utility
{
    public abstract class BaseRepositoryTestBase<TDocument> : IDisposable where TDocument : BaseEntity
    {
        protected readonly MongoDbRunner Runner;
        protected readonly IMongoDatabase Database;
        protected readonly IMongoCollection<TDocument> _collection;        
        protected readonly ILogger<TDocument> Logger;
        protected readonly IOptions<MongoDbSettings> _options;

        protected BaseRepositoryTestBase(string collectionName)
        {
            Runner = MongoDbRunner.Start();
            var client = new MongoClient(Runner.ConnectionString);
            Database = client.GetDatabase("TestDb");

            Logger = new Mock<ILogger<TDocument>>().Object;

            _options = Options.Create(new MongoDbSettings
            {
                ConnectionString = Runner.ConnectionString,
                DatabaseName = "TestDb"
            });

            _collection = Database.GetCollection<TDocument>(collectionName);
        }
        public void Dispose()
        {
            Runner.Dispose();
        }
    }
}

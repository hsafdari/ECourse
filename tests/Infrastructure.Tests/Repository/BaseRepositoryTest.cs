using FluentAssertions;
using Infrastructure.Repository;
using Infrastructure.Tests.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Tests.Repository
{
    public class BaseRepositoryTest : IDisposable
    {
        private readonly MongoDbRunner _mongoRunner;
        private readonly IMongoDatabase _database;
        private readonly BaseRepository<TestEntity> _repository;
        private readonly IMongoCollection<TestEntity> _collection;

        public BaseRepositoryTest()
        {
            _mongoRunner = MongoDbRunner.Start();
            var client = new MongoClient(_mongoRunner.ConnectionString);
            _database = client.GetDatabase("TestDb");

            var logger = new NullLogger<TestEntity>();
            _repository = new BaseRepository<TestEntity>(_database, logger, "TestEntities");
            _collection = _database.GetCollection<TestEntity>("TestEntities");
        }

        [Fact]
        public async Task AddAsync_Should_Insert_Entity()
        {
            var entity = new TestEntity { Name = "Test Name" };

            var result = await _repository.AddAsync(entity);

            var dbEntity = await _collection.Find(x => x.Id == entity.Id).FirstOrDefaultAsync();
            result.Id.Should().NotBeNullOrEmpty();
            dbEntity.Name.Should().Be("Test Name");
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Entities()
        {
            await _collection.InsertManyAsync(new[]
            {
            new TestEntity { Id = ObjectId.GenerateNewId().ToString(), Name = "One" },
            new TestEntity { Id = ObjectId.GenerateNewId().ToString(), Name = "Two" }
        });

            var all = await _repository.GetAllAsync();

            all.Should().HaveCount(2);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Entity()
        {
            var entity = new TestEntity { Id = ObjectId.GenerateNewId().ToString(), Name = "DeleteMe" };
            await _collection.InsertOneAsync(entity);

            var result = await _repository.DeleteAsync(entity.Id);

            result.DeletedCount.Should().Be(1);
        }

        public void Dispose()
        {
            _mongoRunner.Dispose(); // Cleanup
        }
    }
}

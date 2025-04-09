using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using Infrastructure.Repository;
using MongoDB.Driver;

namespace ECourse.Services.CourseAPI.Repositories
{
    public class CourseGroupRepository : BaseRepository<CourseGroup>, ICourseGroupRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMongoCollection<CourseGroup> _collection;
        private readonly ILogger<CourseGroup> _logger;
        public CourseGroupRepository(ApplicationDbContext dbContext, ILogger<CourseGroup> logger) : base(dbContext._database, logger, CourseGroup.DocumentName)
        {
            _context = dbContext;
            _collection = _context._database.GetCollection<CourseGroup>(CourseGroup.DocumentName);
            _logger = logger;
        }
        public async Task<List<CourseGroup>> GetAllTreeAsync()
        {
            return await _collection.FindAsync(x => x.IsDeleted != true && x.ParentId.Equals(null)).Result.ToListAsync();
        }
        public async Task<List<CourseGroup>> GetAllChildrenOfTreeAsync(string parentId)
        {
            var items = await _collection.FindAsync(x => x.IsDeleted != true && x.ParentId.Equals(parentId)).Result.ToListAsync();
            return items;
        }
        public async Task<UpdateResult> SetNodeHasChildren(string parentId)
        {
            try
            {
                if (parentId == null)
                {
                    throw new Exception("ParentId is null");
                }
                return await _collection.UpdateOneAsync(
                Builders<CourseGroup>.Filter.Eq(x => x.Id, parentId),
                Builders<CourseGroup>.Update.Set(x => x.HasChildren, true)
                );
            }
            catch (MongoException mongoEx)
            {
                _logger.LogError(mongoEx, "MongoDB error while updating NodeHasChildren {EntityType}: {Message}", typeof(CourseGroup).Name, mongoEx.Message);
                throw new Exception("A database error occurred while updating NodeHasChildren data.", mongoEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating NodeHasChildren {EntityType}: {Message}", typeof(CourseGroup).Name, ex.Message);
                throw new Exception("An unexpected error occurred while updating NodeHasChildren data.", ex);
            }
        }

        public async Task<UpdateResult> UpdateParentAsync(string destinationParentId, string sourceId)
        {
            //update in the parent
            await SetNodeHasChildren(destinationParentId);
            //check and disable source parent has children
            await SetNodeNotHaveChildren(sourceId);
            //update in the child
            return await _collection.UpdateOneAsync(
                 Builders<CourseGroup>.Filter.Eq(x => x.Id, sourceId),
                 Builders<CourseGroup>.Update.Set(x => x.ParentId, destinationParentId)
             );
           
        }
        private async Task SetNodeNotHaveChildren(string sourceId)
        {
            var sourceItem = await _collection.FindAsync(x => x.Id.Equals(sourceId)).Result.FirstOrDefaultAsync();
            if (sourceItem != null && sourceItem.ParentId != null)
            {
                var allItemsWithSourceParent = await _collection.FindAsync(x => x.ParentId.Equals(sourceItem.ParentId)).Result.ToListAsync();
                if (allItemsWithSourceParent.Count == 1)
                {
                    //update in the child
                    await _collection.UpdateOneAsync(
                         Builders<CourseGroup>.Filter.Eq(x => x.Id, sourceItem.ParentId),
                         Builders<CourseGroup>.Update.Set(x => x.HasChildren, false)
                     );
                }
            }
        }
    }
}

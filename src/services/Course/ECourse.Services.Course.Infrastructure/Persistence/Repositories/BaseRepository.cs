using ECourse.Services.Course.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;

namespace ECourse.Services.Course.Infrastructure.Repository
{
    public class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : BaseEntity
    {
        private readonly IMongoCollection<TModel> _collection;
        private readonly ILogger<TModel> _logger;
        public BaseRepository(IMongoDatabase database, ILogger<TModel> logger, string collectionName)
        {
            _collection = database.GetCollection<TModel>(collectionName);
            _logger = logger;
        }
        #region Get Data
        public async Task<List<TModel>> GetAllAsync()
        {
            try
            {
                return await _collection.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while getting all {EntityType}: {Message}", typeof(TModel).Name, ex.Message);
                throw new Exception("An unexpected error occurred while getting all data.", ex);
            }

        }


        public async Task<TModel> GetByIdAsync(string id)
        {
            try
            {
                return await _collection.Find(Builders<TModel>.Filter.Eq("_id", ObjectId.Parse(id))).FirstOrDefaultAsync();
            }
            catch (MongoException mongoEx)
            {
                _logger.LogError(mongoEx, "MongoDB error while getting {EntityType} id:{id}: {Message}", typeof(TModel).Name, id, mongoEx.Message);
                throw new Exception("A database error occurred while getting data.", mongoEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while getting one {EntityType} id:{id}: {Message}", typeof(TModel).Name, id, ex.Message);
                throw new Exception("An unexpected error occurred while getting one data.", ex);
            }
        }
        #endregion

        #region Add Or Create
        public async Task<TModel> AddAsync(TModel entity)
        {
            try
            {
                entity.Id = ObjectId.GenerateNewId().ToString();
                await _collection.InsertOneAsync(entity);
                return entity;
            }
            catch (MongoException mongoEx)
            {
                _logger.LogError(mongoEx, "MongoDB error while inserting {EntityType}: {Message}", typeof(TModel).Name, mongoEx.Message);
                throw new Exception("A database error occurred while inserting data.", mongoEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while inserting {EntityType}: {Message}", typeof(TModel).Name, ex.Message);
                throw new Exception("An unexpected error occurred while inserting data.", ex);
            }
        }
        #endregion

        #region Update
        public async Task<ReplaceOneResult> UpdateAsync(TModel entity)
        {
            try
            {
                var filter = Builders<TModel>.Filter.Eq(x => x.Id, entity.Id);
                entity.ModifiedDateTime = DateTime.Now;
                return await _collection.ReplaceOneAsync(Builders<TModel>.Filter.Eq("_id", ObjectId.Parse(entity.Id)), entity);
            }
            catch (MongoException mongoEx)
            {
                _logger.LogError(mongoEx, "MongoDB error while updating {EntityType}: {Message}", typeof(TModel).Name, mongoEx.Message);
                throw new Exception("A database error occurred while updating data.", mongoEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating {EntityType}: {Message}", typeof(TModel).Name, ex.Message);
                throw new Exception("An unexpected error occurred while updating data.", ex);
            }
        }
        #endregion

        #region Delete Permanently
        public async Task<DeleteResult> DeleteAsync(string id)
        {
            try
            {
                var result = await _collection.DeleteOneAsync(Builders<TModel>.Filter.Eq("_id", ObjectId.Parse(id)));
                return result;
            }
            catch (MongoException mongoEx)
            {
                _logger.LogError(mongoEx, "MongoDB error while deleting {EntityType} id:{id}: {Message}", typeof(TModel).Name, id, mongoEx.Message);
                throw new Exception("A database error occurred while deleting data.", mongoEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting one {EntityType} id:{id}: {Message}", typeof(TModel).Name, id, ex.Message);
                throw new Exception("An unexpected error occurred while deleting one data.", ex);
            }
        }
        #endregion
        #region MarkAsDelete
        public async Task<UpdateResult> MarkAsDeletedAsync(string id)
        {
            try
            {
                var filter = Builders<TModel>.Filter.Eq(e => e.Id, id);
                var update = Builders<TModel>.Update.Set(e => e.ModifiedDateTime, DateTime.Now)
                    .Set(e => e.IsDeleted, true);
                return await _collection.UpdateOneAsync(filter, update);
            }
            catch (MongoException mongoEx)
            {
                _logger.LogError(mongoEx, "MongoDB error while remove {EntityType}: {Message}", typeof(TModel).Name, mongoEx.Message);
                throw new Exception("A database error occurred while remove data.", mongoEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while remove {EntityType}: {Message}", typeof(TModel).Name, ex.Message);
                throw new Exception("An unexpected error occurred while remove data.", ex);
            }
        }

        public async Task<UpdateResult> MarkAsDeletedAsync(IEnumerable<string> ids)
        {
            var filter = Builders<TModel>.Filter.In(e => e.Id, ids);
            var update = Builders<TModel>.Update.Set(e => e.ModifiedDateTime, DateTime.Now)
                .Set(e => e.IsDeleted, true);
            return await _collection.UpdateManyAsync(filter, update);
        }
        #endregion
        #region GetGridData
        /// <summary>
        /// Get Query from "RadzenGrid" as GridQuery class as a parameter
        /// </summary>
        /// <param name="gridQuery"></param>
        /// <returns></returns>
        public async Task<Utility.PagedResult<TModel>> GetGridDataAsync(GridQuery gridQuery,CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(gridQuery);
            IQueryable<TModel> query = ApplyGridQuery(_collection.AsQueryable(), gridQuery);

            int totalCount = await query.CountAsync(cancellationToken);
            IReadOnlyCollection<TModel> data = await ApplyPagingAsync(gridQuery, query, cancellationToken);

            return new Utility.PagedResult<TModel>
            (
                Items:data.ToList(),
                TotalCount: totalCount,
                Page: gridQuery.page,
                PageSize: gridQuery.top
            );       
        }
        /// <summary>
        /// Apply Paging to the Queryable
        /// </summary>
        /// <param name="gridParameters"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private static async Task<IReadOnlyCollection<TModel>> ApplyPagingAsync(GridQuery gridParameters, IQueryable<TModel> query, CancellationToken cancellationToken)
        {
            // Paging            
            var data = await query.Skip((gridParameters.page - 1) * gridParameters.top).Take(gridParameters.top).ToListAsync(cancellationToken);
            return data.AsReadOnly();

        }
        /// <summary>
        /// Apply Grid Query to the Queryable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="gridParameters"></param>
        /// <returns></returns>
        private IQueryable<T> ApplyGridQuery<T>(IQueryable<T> query, GridQuery gridParameters) where T : BaseEntity
        {
            if (query != null && gridParameters != null)
            {
                query = ApplyGridFilters(query, gridParameters);
                query = ApplyGridSorting(query, gridParameters);                         
                query = query.Where(x => x.IsDeleted == false);
                return query;
            }
            return Enumerable.Empty<T>().AsQueryable();
        }
        /// <summary>
        /// Apply Sorting to the Queryable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="gridParameters"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyGridSorting<T>(IQueryable<T> query, GridQuery gridParameters) where T : BaseEntity
        {
            if (!string.IsNullOrEmpty(gridParameters.sortColumn))
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, gridParameters.sortColumn);
                var lambda = Expression.Lambda(property, parameter);

                var methodName = gridParameters.sortOrder == "asc" ? "OrderBy" : "OrderByDescending";
                var expression = Expression.Call(
                    typeof(Queryable),
                    methodName,
                    new Type[] { typeof(T), property.Type },
                    query.Expression,
                    Expression.Quote(lambda)
                );

                return query.Provider.CreateQuery<T>(expression);
            }

            return query;
        }
        /// <summary>
        /// Apply Filtering to the Queryable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="gridParameters"></param>
        /// <returns></returns>
        private static IQueryable<T> ApplyGridFilters<T>(IQueryable<T> query, GridQuery gridParameters) where T : BaseEntity
        {
            // Filtering
            if (!string.IsNullOrEmpty(gridParameters.filterColumn) && !string.IsNullOrEmpty(gridParameters.filterValue))
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, gridParameters.filterColumn);
                var constant = Expression.Constant(gridParameters.filterValue);
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var containsExpression = Expression.Call(property, containsMethod, constant);
                var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
                query = query.Where(lambda);
            }

            return query;
        }
        #endregion
    }
}

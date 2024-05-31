using Infrastructure.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Infrastructure.Repository
{
    public class BaseRepository<TModel, TRepository> : IBaseRepository<TModel> where TModel : class where TRepository : DbContext
    {
        private readonly IDbContextFactory<TRepository> dbfactory;
        private readonly TRepository _DbSet;

        public BaseRepository(IDbContextFactory<TRepository> datacontext)
        {
            dbfactory = datacontext;
            _DbSet = dbfactory.CreateDbContext();
        }
        public async Task<int> Create(TModel entity)
        {
            try
            {
                using (var _dbContext = dbfactory.CreateDbContext())
                {
                    _dbContext.Set<TModel>().Add(entity);
                    return await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public async Task<int> Delete(TModel entity)
        {
            try
            {
                using (var _dbContext = dbfactory.CreateDbContext())
                {
                    _dbContext.Set<TModel>().Remove(entity);
                    return await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                return -1;
            }
        }

        public async Task<int> DeleteMany(List<TModel> entities)
        {
            try
            {
                using (var _dbContext = dbfactory.CreateDbContext())
                {
                    _dbContext.Set<TModel>().RemoveRange(entities);
                    return await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                return -1;
            }
        }

        public async Task<List<TModel>> GetMany(Expression<Func<TModel, bool>> where)
        {
            List<TModel>? items = null;
            using (var _dbContext = dbfactory.CreateDbContext())
            {
                items = await _dbContext.Set<TModel>().Where(where).ToListAsync();
            }
            //var items2= await _DbSet.Set<TModel>().Where(where).ToListAsync();
            return items;
        }
        public async Task<TModel> GetById(Expression<Func<TModel, bool>> where)
        {
            using (var _dbContext = dbfactory.CreateDbContext())
            {
                return await _dbContext.Set<TModel>().FirstOrDefaultAsync(where);
            }
        }

        public async Task<int> Update(TModel entiry)
        {
            using (var _dbContext = dbfactory.CreateDbContext())
            {
                _dbContext.Set<TModel>().Update(entiry);
                return await _dbContext.SaveChangesAsync();
            }
        }


        public async Task<List<TModel>> Grid(GridQuery query)
        {
            using (var _dbContext = dbfactory.CreateDbContext())
            {
                var items = ApplyQuery(_dbContext.Set<TModel>().AsQueryable(), query);
                return await items.ToDynamicListAsync<TModel>();
            }

        }
        public IQueryable ApplyQuery<T>(IQueryable<T> items, GridQuery query) where T : class
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.filter))
                {
                    //items = items.Where(query.filter);
                    items = items.Where<T>(query.filter);
                }
                if (!string.IsNullOrEmpty(query.orderby))
                {
                    items = items.OrderBy(query.orderby);
                }
                items = items.Skip(query.skip);
                items = items.Take(query.top);
                if (!string.IsNullOrEmpty(query.select))
                {
                    return items.Select($"new ({query.select})");
                }
            }

            return items;
        }
        public IQueryable ApplyQuery<T>(IQueryable<T> items, IQueryCollection? query = null) where T : class
        {
            if (query != null)
            {
                var filter = query.ContainsKey("filter") ? query["filter"].ToString() : null;
                if (!string.IsNullOrEmpty(filter))
                {
                    items = items.Where(filter);
                }

                if (query.ContainsKey("orderBy"))
                {
                    items = items.OrderBy(query["orderBy"].ToString());
                }

                if (query.ContainsKey("skip"))
                {
                    items = items.Skip(int.Parse(query["skip"].ToString()));
                }

                if (query.ContainsKey("top"))
                {
                    items = items.Take(int.Parse(query["top"].ToString()));
                }

                if (query.ContainsKey("select"))
                {
                    return items.Select($"new ({query["select"].ToString()})");
                }
            }

            return items;
        }

        public Task<UpdateResult> Update(Expression<Func<TModel, bool>> where, UpdateDefinition<TModel> update)
        {
            throw new NotImplementedException();
        }
    }
}

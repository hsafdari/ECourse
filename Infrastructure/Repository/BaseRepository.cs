using Infrastructure.Models;
using Infrastructure.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SharpCompress.Common;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Infrastructure.Repository
{
    public class BaseRepository<TModel, TContext> : IBaseRepository<TModel> where TModel : BaseEntity where TContext : DbContext
    {
        private readonly IDbContextFactory<TContext> dbfactory;       

        public BaseRepository(IDbContextFactory<TContext> datacontext)
        {
            dbfactory = datacontext;            
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
        public async Task<int> Delete(Expression<Func<TModel, bool>> where)
        {
            try
            {
                using (var _dbContext = dbfactory.CreateDbContext())
                {
                    var item = await _dbContext.Set<TModel>().Where(where).FirstOrDefaultAsync();
                    if (item!=null)
                    {
                        item.IsDeleted = true;
                        item.ModifiedDateTime = DateTime.Now;
                        _dbContext.Set<TModel>().Update(item);
                    }
                   
                    return await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                return -1;
            }
        }
        /// <summary>
        /// Delete All rows Permanently
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> DeleteMany(Expression<Func<TModel, bool>> where)
        {
            try
            {
                using (var _dbContext = dbfactory.CreateDbContext())
                {
                    var items = await _dbContext.Set<TModel>().Where(where).ToListAsync();
                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            item.IsDeleted = true;
                            item.ModifiedDateTime = DateTime.Now;

                        }                        
                        _dbContext.Set<TModel>().UpdateRange(items);
                    }

                    return await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                return -1;
            }
        }
        public async Task<List<TModel>> GetMany()
        {
            List<TModel>? items = null;
            using (var _dbContext = dbfactory.CreateDbContext())
            {

                items = await _dbContext.Set<TModel>().Where(x => x.IsDeleted == false).ToListAsync();
            }            
            return items;
        }
        public async Task<List<TModel>> GetMany(Expression<Func<TModel, bool>> where)
        {
            List<TModel>? items = null;
            using (var _dbContext = dbfactory.CreateDbContext())
            {

                items = await _dbContext.Set<TModel>().Where(x=>x.IsDeleted==false).Where(where).ToListAsync();
            }
            //var items2= await _DbSet.Set<TModel>().Where(where).ToListAsync();
            return items;
        }
        public async Task<TModel> GetById(Expression<Func<TModel, bool>> where)
        {
            using (var _dbContext = dbfactory.CreateDbContext())
            {
                return await _dbContext.Set<TModel>().Where(x => x.IsDeleted == false).Where(where).FirstOrDefaultAsync();
            }
        }

        public async Task<int> Update(TModel entity)
        {
            using (var _dbContext = dbfactory.CreateDbContext())
            {
                _dbContext.Set<TModel>().Update(entity);
                return await _dbContext.SaveChangesAsync();
            }
        }


        public async Task<(List<TModel>,int)> Grid(GridQuery query)
        {
            using (var _dbContext = dbfactory.CreateDbContext())
            {
                var items= ApplyQuery(_dbContext.Set<TModel>().AsQueryable(), query);               
                int count=items.Count();
                items = items.Skip(query.skip).Take(query.top);
                return (await items.ToDynamicListAsync<TModel>(), count);
            }

        }
        private IQueryable<T> ApplyQuery<T>(IQueryable<T> items, GridQuery query) where T : BaseEntity
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.filter))
                {                    
                    items = items.Where<T>(query.filter);
                }
                if (!string.IsNullOrEmpty(query.orderby))
                {
                    items = items.OrderBy(query.orderby);
                }

                //we should ignore take and skip here, because we to need calculate total records for grid paging
                //in controller we use take and return data to client
                //items = items.Skip(query.skip);
                //items = items.Take(query.top);                
                items = items.Where(x => x.IsDeleted == false);
            }

            return items;
        }      
    }
}

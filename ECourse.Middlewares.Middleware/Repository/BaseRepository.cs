using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Middleware.Utility;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using MongoDB.EntityFrameworkCore.Extensions;
using MongoDB.Driver.Core.Operations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;

namespace Middleware.Repository
{
    public class BaseRepository<TModel,TRepository> : IBaseRepository<TModel> where TModel : class where TRepository: DbContext
    {
        //private readonly IMongoCollection<TModel> model = db.GetCollection<TModel>(documentname);
        private DbSet<TModel> Dbset { get; set; }
        private readonly IDbContextFactory<TRepository> dbfactory;
        public BaseRepository(IDbContextFactory<TRepository> datacontext)
        {
            dbfactory = datacontext;
            using (var _dbContext = dbfactory.CreateDbContext())
            {
                Dbset = _dbContext.Set<TModel>(nameof(TModel));
                
            }
        }
        public async Task<int> Create(TModel entity)
        {
            try
            {
                using (var _dbContext = dbfactory.CreateDbContext())
                {
                    Dbset.Entry(entity);
                    return await _dbContext.SaveChangesAsync();
                    //await model.InsertOneAsync(entity);
                    // return 1;
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
                //await model.DeleteOneAsync<TModel>(where);                
                return 1;
            }
            catch (Exception)
            {

                return -1;
            }
        }

        public async Task<int> DeleteMany(Expression<Func<TModel, bool>> where)
        {
            try
            {
                //await model.DeleteManyAsync<TModel>(where);               
                return 1;
            }
            catch (Exception)
            {

                return -1;
            }
        }

        public async Task<List<TModel>> GetMany(Expression<Func<TModel, bool>> where)
        {
            using (var _dbContext = dbfactory.CreateDbContext())
            {
               return await _dbContext.Set<TModel>().Where(where).ToListAsync();
            }
        }
        public async Task<TModel> GetById(Expression<Func<TModel, bool>> where) =>
            await Dbset.Where(where).FirstOrDefaultAsync();

        public async Task<UpdateResult> Update(TModel entiry)
        {
            //Dbset.Update(entiry);
            //DataContext.SaveChanges();
            throw new NotImplementedException();
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
        public IQueryable ApplyQuery<T>(IQueryable<T> items, IQueryCollection query = null) where T : class
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

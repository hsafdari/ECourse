using Microsoft.AspNetCore.Http;
using Middleware.Utility;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Middleware.Repository
{
    public interface IBaseRepository<TModel> where TModel : class
    {
        Task<int> Create(TModel entity);           
        Task<TModel> GetById(Expression<Func<TModel, bool>> where);
        Task<List<TModel>> GetMany(Expression<Func<TModel, bool>> where);
        Task<UpdateResult> Update(Expression<Func<TModel, bool>> where, UpdateDefinition<TModel> update);
        Task<int> Delete(Expression<Func<TModel, bool>> where);
        Task<int> DeleteMany(Expression<Func<TModel, bool>> where);
        Task<List<TModel>> Grid(GridQuery query);
        

    }
}

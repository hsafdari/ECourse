using Infrastructure.Utility;
using System.Linq.Expressions;

namespace Infrastructure.Repository
{
    public interface IBaseRepository<TModel> where TModel : class
    {
        Task<int> Create(TModel entity);
        Task<TModel> GetById(Expression<Func<TModel, bool>> where);
        Task<List<TModel>> GetMany(Expression<Func<TModel, bool>> where);
        Task<int> Update(TModel entity);
        Task<int> Delete(TModel entity);
        Task<int> DeleteMany(List<TModel> entities);
        Task<List<TModel>> Grid(GridQuery query);


    }
}

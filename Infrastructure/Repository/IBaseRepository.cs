using Infrastructure.Utility;
using System.Linq.Expressions;

namespace Infrastructure.Repository
{
    public interface IBaseRepository<TModel> where TModel : class
    {
        Task<int> Create(TModel entity);
        Task<TModel> GetById(Expression<Func<TModel, bool>> where);
        Task<List<TModel>> GetMany();
        Task<List<TModel>> GetMany(Expression<Func<TModel, bool>> where);
        Task<int> Update(TModel entity);
        /// <summary>
        /// remove a row temporary
        /// </summary>
        /// <param name="where">condition</param>
        /// <returns></returns>
        Task<int> Delete(Expression<Func<TModel, bool>> where);
        /// <summary>
        /// remove rows temporary
        /// </summary>
        /// <param name="where">condition</param>
        /// <returns></returns>
        Task<int> DeleteMany(Expression<Func<TModel, bool>> where);
        Task<(List<TModel>, int)> Grid(GridQuery query);


    }
}

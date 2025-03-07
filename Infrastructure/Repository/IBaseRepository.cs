using Infrastructure.Utility;
using MongoDB.Driver;

namespace Infrastructure.Repository
{
    public interface IBaseRepository<TModel> where TModel : class
    {
        Task<List<TModel>> GetAllAsync();
        Task<TModel> GetByIdAsync(string id);
        Task<TModel> AddAsync(TModel entity);
        Task<ReplaceOneResult> UpdateAsync(TModel entity);
        Task<DeleteResult> DeleteAsync(string id);
        //use as soft delete and data removed from grid but exist in Database
        Task<UpdateResult> MarkAsDeletedAsync(string id);
        //use as soft delete bunch of Ids and data removed from grid but exist in Database
        Task<UpdateResult> MarkAsDeletedAsync(IEnumerable<string> ids);

        Task<(List<TModel>, long)> GetGridDataAsync(GridQuery query);
    }
}

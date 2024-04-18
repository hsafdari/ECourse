using ECourse.Admin.Models;

namespace ECourse.Admin.Service
{
    public interface IBaseService<TModel> where TModel : class
    {
        Task<ResponseDto?> CreateAsync(TModel entity);
        Task<ResponseDto?> GetAllAsync();
        Task<ResponseDto?> GetByIdAsync(string id);
        Task<ResponseDto?> UpdateAsync(TModel entity);
        Task<ResponseDto?> DeleteAsync(string id);
    }
}

﻿using ECourse.Admin.Models;
using Radzen;

namespace ECourse.Admin.Service
{
    public interface IBaseService<TModel> where TModel : class
    {
        Task<ResponseDto?> CreateAsync(TModel entity);
        Task<ResponseDto?> GetAllAsync();
        Task<ResponseDto?> GetByIdAsync(string id);
        Task<ResponseDto?> UpdateAsync(TModel entity);
        Task<ResponseDto?> DeleteAsync(string id);
        Task<ResponseDto?> DeleteAsync(List<string> ids);
        Task<ResponseDto?> GetGrid(Query query);
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.VehicleService.v1.Categories
{
    public interface ICategoryAppService
    {
        Task<CategoryDto> CreateAsync(CreateCategoryDto values);
        Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto input);
        Task DeleteAsync(Guid id);
        Task UndoAsync(Guid id);
        Task<CategoryDto> GetAsync(Guid id, bool isDeleted = false);
        Task<List<CategoryDto>> GetListAsync(bool isDeleted = false);
        //Task<List<CategoryDto>> GetWithDeletedListAsync();
    }
}
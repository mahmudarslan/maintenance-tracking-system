using Arslan.Vms.VehicleService.v1.VehicleTypes.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.VehicleService.v1.VehicleTypes
{
    public interface IVehicleTypeAppService
    {
        Task<VehicleTypeDto> CreateAsync(CreateVehicleTypeDto input);
        Task<VehicleTypeDto> UpdateAsync(Guid id, UpdateVehicleTypeDto input);
        Task DeleteAsync(Guid id);
        Task UndoAsync(Guid id);
        Task<VehicleTypeDto> GetAsync(Guid id, bool isDeleted = false);
        Task<List<VehicleTypeDto>> GetListAsync(bool isDeleted = false);
        Task<List<BrandDto>> GetBrandListAsync();
        Task<List<ModelDto>> GetModelsByBrandAsync(Guid brandId);
        Task<List<ModelDto>> GetAllModelsAsync();
    }
}
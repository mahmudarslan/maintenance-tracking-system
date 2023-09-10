using Arslan.Vms.VehicleService.v1.Vehicles.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using System;
using System.Threading.Tasks;

namespace Arslan.Vms.VehicleService.v1.Vehicles
{
    public interface IVehicleAppService
    {
        Task<VehicleDto> CreateAsync(CreateVehicleDto input);
        Task<VehicleDto> UpdateAsync(Guid id, UpdateVehicleDto input);
        Task DeleteAsync(Guid userId, Guid vehicleId);
        Task UndoAsync(Guid userId, Guid vehicleId);
        Task<VehicleDto> GetAsync(Guid id, bool isDeleted = false);
        Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions);
    }
}
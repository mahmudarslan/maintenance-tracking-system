using Arslan.Vms.VehicleService.v1.Locations.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.VehicleService.v1.Locations
{
    public interface ILocationAppService
    {
        Task<LocationDto> CreateAsync(CreateLocationDto input);
        Task<LocationDto> UpdateAsync(Guid id, UpdateLocationDto input);
        Task DeleteAsync(Guid id);
        Task UndoAsync(Guid id);
        Task<LocationDto> GetAsync(Guid id, bool isDeleted = false);
        Task<List<LocationDto>> GetListAsync(bool isDeleted = false);
        //Task<List<LocationDto>> GetWithDeletedListAsync();
    }
}
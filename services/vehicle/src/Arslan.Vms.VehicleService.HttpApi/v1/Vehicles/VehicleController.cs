using Arslan.Vms.VehicleService.v1.Vehicles.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Arslan.Vms.VehicleService.v1.Vehicles
{
    [ApiVersion("1.0")]
    [ApiController]
    [Area("Base")]
    [ControllerName("Vehicle")]
    [Route("vehicle/v{version:apiVersion}/vehicle")]
    public class VehicleController : VehicleServiceController, IVehicleAppService
    {
        protected IVehicleAppService _vehicleAppService { get; }

        public VehicleController(IVehicleAppService vehicleAppService)
        {
            _vehicleAppService = vehicleAppService;
        }

        [HttpPost]
        public virtual Task<VehicleDto> CreateAsync(CreateVehicleDto input)
        {
            return _vehicleAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<VehicleDto> UpdateAsync(Guid id, UpdateVehicleDto input)
        {
            return _vehicleAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid userId, Guid vehicleId)
        {
            return _vehicleAppService.DeleteAsync(userId, vehicleId);
        }

        [HttpPost]
        [Route("Undo/{id}")]
        public virtual Task UndoAsync(Guid userId, Guid vehicleId)
        {
            return _vehicleAppService.UndoAsync(userId, vehicleId);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<VehicleDto> GetAsync(Guid id, bool isDeleted)
        {
            return _vehicleAppService.GetAsync(id, isDeleted);
        }

        [HttpGet]
        public virtual Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            return _vehicleAppService.GetListAsync(loadOptions);
        }
    }
}
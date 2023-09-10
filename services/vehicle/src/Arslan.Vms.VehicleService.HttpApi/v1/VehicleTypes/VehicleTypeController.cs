using Arslan.Vms.VehicleService.v1.VehicleTypes.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.VehicleService.v1.VehicleTypes
{
    [ApiVersion("1.0")]
    [ApiController]
    [Area("Base")]
    [ControllerName("VehicleType")]
    [Route("vehicle/v{version:apiVersion}/vehicleType")]
    public class VehicleTypeController : VehicleServiceController, IVehicleTypeAppService
    {
        protected IVehicleTypeAppService _vehicleAppService { get; }

        public VehicleTypeController(IVehicleTypeAppService vehicleAppService)
        {
            _vehicleAppService = vehicleAppService;
        }

        [HttpPost]
        public virtual Task<VehicleTypeDto> CreateAsync(CreateVehicleTypeDto input)
        {
            return _vehicleAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<VehicleTypeDto> UpdateAsync(Guid id, UpdateVehicleTypeDto input)
        {
            return _vehicleAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _vehicleAppService.DeleteAsync(id);
        }

        [HttpPost]
        [Route("Undo/{id}")]
        public Task UndoAsync(Guid id)
        {
            return _vehicleAppService.UndoAsync(id);
        }

        [HttpGet]
        [Route("{id}/{isDeleted}")]
        public Task<VehicleTypeDto> GetAsync(Guid id, bool isDeleted)
        {
            return _vehicleAppService.GetAsync(id, isDeleted);
        }

        [HttpGet]
        [Route("{withDeleted}")]
        public virtual Task<List<VehicleTypeDto>> GetListAsync(bool withDeleted)
        {
            return _vehicleAppService.GetListAsync(withDeleted);
        }

        //[HttpGet]
        //[Route("WithDeleteds")]
        //public virtual Task<List<CreateUpdateVehicleTreeModelDto>> GetWithDeletedListAsync()
        //{
        //    return _vehicleAppService.GetWithDeletedListAsync();
        //}

        [HttpGet]
        [Route("Brands")]
        public virtual Task<List<BrandDto>> GetBrandListAsync()
        {
            return _vehicleAppService.GetBrandListAsync();
        }

        [HttpGet]
        [Route("ModelsByBrand/{brandId}")]
        public virtual Task<List<ModelDto>> GetModelsByBrandAsync(Guid brandId)
        {
            return _vehicleAppService.GetModelsByBrandAsync(brandId);
        }

        [HttpGet]
        [Route("AllModels")]
        public virtual Task<List<ModelDto>> GetAllModelsAsync()
        {
            return _vehicleAppService.GetAllModelsAsync();
        }


    }
}
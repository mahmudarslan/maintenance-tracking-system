using Arslan.Vms.ProductService.v1.Locations.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.ProductService.v1.Locations
{
    [ApiController]
    //[RemoteService(Name = "ArslanVmsInventory")]
    [Area("Base")]
    [ControllerName("Location")]
    [Route("rest/api/latest/vms/base/location")]
    //[ApiVersion("1.0")]
    public class LocationController : ProductServiceController, ILocationAppService
    {
        protected ILocationAppService _locationAppService { get; }

        public LocationController(ILocationAppService locationAppService)
        {
            _locationAppService = locationAppService;
        }

        [HttpPost]
        public Task<LocationDto> CreateAsync(CreateLocationDto input)
        {
            return _locationAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<LocationDto> UpdateAsync(Guid id, UpdateLocationDto input)
        {
            return _locationAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _locationAppService.DeleteAsync(id);
        }

        [HttpPost]
        [Route("Undo/{id}")]
        public Task UndoAsync(Guid id)
        {
            return _locationAppService.UndoAsync(id);
        }

        [HttpGet]
        [Route("{id}/{isDeleted}")]
        public Task<LocationDto> GetAsync(Guid id, bool isDeleted = false)
        {
            return _locationAppService.GetAsync(id);
        }

        [HttpGet]
        [Route("{withDeleted}")]
        public virtual Task<List<LocationDto>> GetListAsync(bool withDeleted = false)
        {
            return _locationAppService.GetListAsync(withDeleted);
        }

        //[HttpGet]
        //[Route("WithDeleteds")]
        //public virtual Task<List<LocationDto>> GetWithDeletedListAsync()
        //{
        //    return _locationAppService.GetListAsync(true);
        //}
    }
}
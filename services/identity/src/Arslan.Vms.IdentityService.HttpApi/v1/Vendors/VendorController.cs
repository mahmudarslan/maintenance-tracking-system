using Arslan.Vms.IdentityService.v1.Vendors.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.IdentityService.v1.Vendors
{
	[ApiVersion("1.0")]
    [ApiController]
    [Area("Base")]
    [ControllerName("Vendor")]
    [Route("identity/v{version:apiVersion}/vms/base/vendor")]
    public class VendorController : IdentityServiceController, IVendorAppService
    {
        protected IVendorAppService _vendorAppService { get; }

        public VendorController(IVendorAppService vendorAppService)
        {
            _vendorAppService = vendorAppService;
        }

        [HttpPost]
        public Task<VendorDto> CreateAsync(CreateVendorDto input)
        {
            ValidateModel();
            return _vendorAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<VendorDto> UpdateAsync(Guid id, UpdateVendorDto input)
        {
            return _vendorAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        public virtual Task DeleteAsync([FromForm(Name = "key")] string key)
        {
            return _vendorAppService.DeleteAsync(key);
        }

        [HttpPost]
        [Route("Undo")]
        public virtual Task UndoAsync(Guid id)
        {
            return _vendorAppService.UndoAsync(id);
        }

        [HttpGet]
        [Route("{id}/{isDeleted}")]
        public virtual Task<VendorDto> GetAsync(Guid id, bool isDeleted = false)
        {
            return _vendorAppService.GetAsync(id, isDeleted);
        }

        [HttpGet]
        public virtual Task<LoadResult> GetListAsync(DataSourceLoadOptions input)
        {
            return _vendorAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("BySearchContent")]
        public Task<List<VendorSearchDto>> GetBySearchContent(string nameSurnameGsm)
        {
            return _vendorAppService.GetBySearchContent(nameSurnameGsm);
        }

        [HttpGet]
        [Route("ByVendorId")]
        public Task<VendorSearchDto> GetByVendorId(Guid id)
        {
            return _vendorAppService.GetByVendorId(id);
        }
    }
}
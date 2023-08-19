using Arslan.Vms.IdentityService.v1.AddressTypes;
using Arslan.Vms.IdentityService.v1.AddressTypes.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.IdentityService.v1.AddressType
{
    [ApiController]
    //[RemoteService(Name = "ArslanVmsIdentity")]
    [Area("Base")]
    [ControllerName("AddressType")]
    [Route("rest/api/latest/vms/base/address")]
    //[ApiVersion("1.0")]
    public class AddressTypeController : AdministrationServiceController, IAddressTypeAppService
    {
        protected IAddressTypeAppService _addressTypeAppService { get; }

        public AddressTypeController(IAddressTypeAppService addressAppService)
        {
            _addressTypeAppService = addressAppService;
        }

        [HttpPost]
        public virtual Task<AddressTypeDto> CreateAsync(CreateAddressTypeDto input)
        {
            return _addressTypeAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<AddressTypeDto> UpdateAsync(Guid id, UpdateAddressTypeDto input)
        {
            return _addressTypeAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _addressTypeAppService.DeleteAsync(id);
        }

        [HttpPost]
        [Route("Undo/{id}")]
        public Task UndoAsync(Guid id)
        {
            return _addressTypeAppService.UndoAsync(id);
        }

        [HttpGet]
        [Route("{id}/{isDeleted}")]
        public Task<AddressTypeDto> GetAsync(Guid id, bool isDeleted = false)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{withDeleted}")]
        public virtual Task<List<AddressTypeDto>> GetListAsync(bool withDeleted)
        {
            return _addressTypeAppService.GetListAsync();
        }


        [HttpGet]
        [Route("DistrictsByCity/{city}")]
        public virtual Task<List<DistrictDto>> GetDistrictsByCityAsync(Guid city)
        {
            return _addressTypeAppService.GetDistrictsByCityAsync(city);
        }

        [HttpGet]
        [Route("Countries")]
        public virtual Task<List<CountryDto>> GetCountryListAsync()
        {
            return _addressTypeAppService.GetCountryListAsync();
        }

        [HttpGet]
        [Route("CitiesByCountry/{country}")]
        public virtual Task<List<CityDto>> GetCitiesByCountryAsync(Guid country)
        {
            return _addressTypeAppService.GetCitiesByCountryAsync(country);
        }
    }
}

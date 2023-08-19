using Arslan.Vms.IdentityService.v1.AddressTypes.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.IdentityService.v1.Addresses
{
    public interface IAddressAppService
    {
        Task CreateAsync(string values);
        Task UpdateAsync(string values, string key);
        Task DeleteAsync(string key);
        Task<List<AddressTypeDto>> GetListAsync();
        Task<List<AddressTypeDto>> GetWithDeletedListAsync();
        Task<List<CityDto>> GetCitiesByCountryAsync(Guid country);
        Task<List<CountryDto>> GetCountryListAsync();
        Task<List<DistrictDto>> GetDistrictsByCityAsync(Guid city);
    }
}
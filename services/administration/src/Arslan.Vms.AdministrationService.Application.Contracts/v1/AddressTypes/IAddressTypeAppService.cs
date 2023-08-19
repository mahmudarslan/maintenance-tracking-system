using Arslan.Vms.AdministrationService.v1.AddressTypes.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.AdministrationService.v1.AddressTypes
{
    public interface IAddressTypeAppService
    {
        Task<AddressTypeDto> CreateAsync(CreateAddressTypeDto input);
        Task<AddressTypeDto> UpdateAsync(Guid id, UpdateAddressTypeDto input);
        Task DeleteAsync(Guid id);
        Task UndoAsync(Guid id);
        Task<AddressTypeDto> GetAsync(Guid id, bool isDeleted = false);
        Task<List<AddressTypeDto>> GetListAsync(bool isDeleted = false);
        Task<List<CityDto>> GetCitiesByCountryAsync(Guid country);
        Task<List<CountryDto>> GetCountryListAsync();
        Task<List<DistrictDto>> GetDistrictsByCityAsync(Guid city);
    }
}
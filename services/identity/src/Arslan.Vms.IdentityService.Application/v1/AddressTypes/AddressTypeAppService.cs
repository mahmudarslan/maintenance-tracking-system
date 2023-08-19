using Arslan.Vms.IdentityService.Addresses.AddressTypes;
using Arslan.Vms.IdentityService.Permissions;
using Arslan.Vms.IdentityService.v1.AddressTypes;
using Arslan.Vms.IdentityService.v1.AddressTypes.Dto;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.IdentityService.v1.AddressTypes
{
    [Authorize(AdministrationServicePermissions.AddressType.Default)]
    public class AddressTypeAppService : AdministrationServiceAppService, IAddressTypeAppService
    {
        private readonly IAddressTypeRepository _addressTypeRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        //private readonly IStringLocalizer<VehicleServiceResource> _localizer;
        private readonly IDataFilter _dataFilter;

        public AddressTypeAppService(IAddressTypeRepository addressTreeRepository,
            IDataFilter dataFilter,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant
            //IStringLocalizer<VehicleServiceResource> localizer
            )
        {
            _addressTypeRepository = addressTreeRepository;
            _currentTenant = currentTenant;
            _dataFilter = dataFilter;
            //_localizer = localizer;
            _guidGenerator = guidGenerator;
        }

        [Authorize(AdministrationServicePermissions.AddressType.Create)]
        public async Task<AddressTypeDto> CreateAsync(CreateAddressTypeDto input)
        {
            //aynı isimden iki kayıt olamaz (pasif kayıtlar da dahil)
            //boş isim olamaz
            //

            using (_dataFilter.Disable<ISoftDelete>())
            {
                var locations = await _addressTypeRepository.ToListAsync();

                if (locations.Any(a => a.Name == input.Name))
                {
                    throw new UserFriendlyException(L["Error:AddressTypeShouldBeUnique"].Value);
                };

                if (string.IsNullOrEmpty(input.Name))
                {
                    throw new UserFriendlyException(L["Error:AddressTypeShouldNotBeEmpty"].Value);
                };

                var level = 1;
                if (input.ParentId != null)
                {
                    var gg = locations.FirstOrDefault(f => f.Id == input.ParentId);
                    level = gg.Level + 1;
                }

                var data = new AddressType(_guidGenerator.Create(), _currentTenant.Id, input.ParentId, input.Name, level);
                await _addressTypeRepository.InsertAsync(data, true);

                var result = ObjectMapper.Map<AddressType, AddressTypeDto>(data);
                return result;
            }
        }

        [Authorize(AdministrationServicePermissions.AddressType.Update)]
        public async Task<AddressTypeDto> UpdateAsync(Guid id, UpdateAddressTypeDto input)
        {
            //aynı isimden iki kayıt olamaz (pasif kayıtlar da dahil)
            //boş isim olamaz

            using (_dataFilter.Disable<ISoftDelete>())
            {
                var categories = await _addressTypeRepository.ToListAsync();

                if (categories.Any(a => a.Id != id && a.Name == input.Name))
                {
                    throw new UserFriendlyException(L["Error:AddressTypeShouldBeUnique"].Value);
                };

                if (string.IsNullOrEmpty(input.Name))
                {
                    throw new UserFriendlyException(L["Error:AddressTypeShouldNotBeEmpty"].Value);
                };

                var level = 1;
                if (input.ParentId != null)
                {
                    var gg = categories.FirstOrDefault(f => f.Id == input.ParentId);
                    level = gg.Level + 1;
                }


                var dbData = categories.FirstOrDefault(f => f.Id == id);
                dbData.Set(input.ParentId, input.Name, level);
                //dbData.Name = input.Name;
                //dbData.ParentId = input.ParentId;
                //dbData.Level = level;
                await _addressTypeRepository.UpdateAsync(dbData, true);

                var result = ObjectMapper.Map<AddressType, AddressTypeDto>(dbData);
                return result;
            }
        }

        [Authorize(AdministrationServicePermissions.AddressType.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            //son kayıt silinemez
            //alt kayıtları bulunan bir sonraki yere aktarılır
            //alt kategori olan silinemez

            using (_dataFilter.Disable<ISoftDelete>())
            {

                var categories = await _addressTypeRepository.ToListAsync();
                var category = categories.FirstOrDefault(f => f.Id == id);

                //son kayıt silinemez
                if (categories.Where(w => w.IsDeleted == false).Count() == 1)
                {
                    throw new UserFriendlyException(L["Error:AddressTypeLeastOne"].Value);
                };

                //alt kategorisi olan silinemez
                if (categories.Any(a => a.IsDeleted == false && a.ParentId == id))
                {
                    throw new UserFriendlyException(L["Error:AddressTypeCannotDeleteSubCategory"].Value);
                };

                //kayıtları olan kategory parenta aktarılır
                //if (category.ParentId != null)
                //{
                //    var products = _locationRepository.Where(w => w.ProductCategoryId == id).ToList();
                //    foreach (var product in products)
                //    {
                //        product.ProductCategoryId = category.ParentId.Value;
                //        await _locationRepository.UpdateAsync(product);
                //    }
                //}

                await _addressTypeRepository.DeleteAsync(d => d.Id == id);
            }
        }

        [Authorize(AdministrationServicePermissions.AddressType.Undo)]
        public async Task UndoAsync(Guid id)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var dbData = await _addressTypeRepository.FirstOrDefaultAsync(f => f.Id == id);
                dbData.IsDeleted = false;
                await _addressTypeRepository.UpdateAsync(dbData, true);

                //parent silinmiş ise undo yap
                if (dbData.ParentId != null)
                {
                    var parentCategory = await _addressTypeRepository.FirstOrDefaultAsync(f => f.Id == dbData.ParentId);

                    if (parentCategory.IsDeleted == true)
                    {
                        parentCategory.IsDeleted = false;
                        await _addressTypeRepository.UpdateAsync(parentCategory, true);
                    }
                }
            }
        }

        public async Task<AddressTypeDto> GetAsync(Guid id, bool isDeleted)
        {
            using (isDeleted == true ? _dataFilter.Disable<ISoftDelete>() : _dataFilter.Enable<ISoftDelete>())
            {
                var addressTypeRepository = await _addressTypeRepository.GetQueryableAsync();

                var result = (from vt in addressTypeRepository
                              where vt.Id == id
                              select new AddressTypeDto
                              {
                                  Name = vt.Name,
                                  Id = vt.Id,
                                  ParentId = vt.ParentId,
                                  Level = vt.Level,
                                  IsDeleted = vt.IsDeleted
                              }).FirstOrDefault();

                return await Task.FromResult(result);
            }
        }

        [Authorize(AdministrationServicePermissions.AddressType.List)]
        public async Task<List<AddressTypeDto>> GetListAsync(bool isDeleted = false)
        {
            using (isDeleted == true ? _dataFilter.Disable<ISoftDelete>() : _dataFilter.Enable<ISoftDelete>())
            {
                var addressTypeRepository = await _addressTypeRepository.GetQueryableAsync();

                var result = (from vt in addressTypeRepository
                              select new AddressTypeDto
                              {
                                  Name = vt.Name,
                                  Id = vt.Id,
                                  ParentId = vt.ParentId,
                                  Level = vt.Level,
                                  IsDeleted = vt.IsDeleted
                              }).OrderBy(o => o.Name).ToList();

                return await Task.FromResult(result);
            }
        }

        //public async Task<List<AddressTypeDto>> GetWithDeletedListAsync()
        //{
        //    using (_dataFilter.Disable<ISoftDelete>())
        //    {
        //        var result = (from vt in _addressTreeRepository
        //                      select new AddressTypeDto
        //                      {
        //                          Name = vt.Name,
        //                          Id = vt.Id,
        //                          ParentId = vt.ParentId,
        //                          IsDeleted = vt.IsDeleted
        //                      }).OrderBy(o => o.Name).ToList();

        //        return await Task.FromResult(result);
        //    }
        //}

        public async Task<List<CountryDto>> GetCountryListAsync()
        {
            var addressTypeRepository = await _addressTypeRepository.GetQueryableAsync();

            var result = addressTypeRepository.Where(w => w.Level == 1 && w.ParentId == null)
                .Select(s => new CountryDto
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
            return await Task.FromResult(result);
        }

        public async Task<List<CityDto>> GetCitiesByCountryAsync(Guid countryId)
        {
            var addressTypeRepository = await _addressTypeRepository.GetQueryableAsync();

            var result = addressTypeRepository.Where(w => w.ParentId == countryId && w.Level == 2)
                .Select(s => new CityDto
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
            return await Task.FromResult(result);
        }

        public async Task<List<DistrictDto>> GetDistrictsByCityAsync(Guid cityId)
        {
            var addressTypeRepository = await _addressTypeRepository.GetQueryableAsync();

            var result = addressTypeRepository.Where(w => w.ParentId == cityId && w.Level == 3)
                .Select(s => new DistrictDto
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
            return await Task.FromResult(result);
        }
    }
}
using Arslan.Vms.ProductService.Localization;
using Arslan.Vms.ProductService.Locations;
using Arslan.Vms.ProductService.Permissions;
using Arslan.Vms.ProductService.v1.Locations.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.v1.Locations
{
    [Authorize(ProductServicePermissions.Location.Default)]
    public class LocationAppService : ProductServiceAppService, ILocationAppService
    {
        #region Fields
        private readonly ILocationRepository _locationRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IStringLocalizer<ProductServiceResource> _localizer;
        private readonly IDataFilter _dataFilter;
        #endregion

        #region Ctor
        public LocationAppService(
      ILocationRepository locationRepository,
      ICurrentTenant currentTenant,
      IGuidGenerator guidGenerator,
      IDataFilter dataFilter,
       IStringLocalizer<ProductServiceResource> localizer
    )
        {
            _locationRepository = locationRepository;
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;
            _localizer = localizer;
            _dataFilter = dataFilter;
        }
        #endregion

        [Authorize(ProductServicePermissions.Location.Create)]
        public async Task<LocationDto> CreateAsync(CreateLocationDto input)
        {
            //aynı isimden iki kayıt olamaz (pasif kayıtlar da dahil)
            //boş isim olamaz
            //

            using (_dataFilter.Disable<ISoftDelete>())
            {
                var locations = await _locationRepository.ToListAsync();

                if (locations.Any(a => a.Name == input.Name))
                {
                    throw new UserFriendlyException(_localizer["Error:LocationShouldBeUnique"].Value);
                };

                if (string.IsNullOrEmpty(input.Name))
                {
                    throw new UserFriendlyException(_localizer["Error:LocationShouldNotBeEmpty"].Value);
                };

                var level = 1;
                if (input.ParentId != null)
                {
                    var gg = locations.FirstOrDefault(f => f.Id == input.ParentId);
                    level = gg.Level + 1;
                }

                var data = new Location(_guidGenerator.Create(), _currentTenant.Id, input.ParentId, input.Name, level);
                await _locationRepository.InsertAsync(data, true);

                var result = ObjectMapper.Map<Location, LocationDto>(data);
                return result;
            }
        }

        [Authorize(ProductServicePermissions.Location.Update)]
        public async Task<LocationDto> UpdateAsync(Guid id, UpdateLocationDto input)
        {
            //aynı isimden iki kayıt olamaz (pasif kayıtlar da dahil)
            //boş isim olamaz

            using (_dataFilter.Disable<ISoftDelete>())
            {
                var categories = await _locationRepository.ToListAsync();

                if (categories.Any(a => a.Id != id && a.Name == input.Name))
                {
                    throw new UserFriendlyException(_localizer["Error:LocationShouldBeUnique"].Value);
                };

                if (string.IsNullOrEmpty(input.Name))
                {
                    throw new UserFriendlyException(_localizer["Error:LocationShouldNotBeEmpty"].Value);
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
                await _locationRepository.UpdateAsync(dbData, true);

                var result = ObjectMapper.Map<Location, LocationDto>(dbData);
                return result;
            }
        }

        [Authorize(ProductServicePermissions.Location.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            //son kayıt silinemez
            //alt kayıtları bulunan bir sonraki yere aktarılır
            //alt kategori olan silinemez

            using (_dataFilter.Disable<ISoftDelete>())
            {

                var categories = await _locationRepository.ToListAsync();
                var category = categories.FirstOrDefault(f => f.Id == id);

                //son kayıt silinemez
                if (categories.Where(w => w.IsDeleted == false).Count() == 1)
                {
                    throw new UserFriendlyException(_localizer["Error:LocationLeastOne"].Value);
                };

                //alt kategorisi olan silinemez
                if (categories.Any(a => a.IsDeleted == false && a.ParentId == id))
                {
                    throw new UserFriendlyException(_localizer["Error:LocationCannotDeleteSubCategory"].Value);
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

                await _locationRepository.DeleteAsync(d => d.Id == id);
            }
        }

        [Authorize(ProductServicePermissions.Location.Undo)]
        public async Task UndoAsync(Guid id)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var dbData = await _locationRepository.FirstOrDefaultAsync(f => f.Id == id);
                dbData.IsDeleted = false;
                await _locationRepository.UpdateAsync(dbData, true);

                //parent silinmiş ise undo yap
                if (dbData.ParentId != null)
                {
                    var parentCategory = await _locationRepository.FirstOrDefaultAsync(f => f.Id == dbData.ParentId);

                    if (parentCategory.IsDeleted == true)
                    {
                        parentCategory.IsDeleted = false;
                        await _locationRepository.UpdateAsync(parentCategory, true);
                    }
                }
            }
        }

        public async Task<LocationDto> GetAsync(Guid id, bool isDeleted)
        {
            using (isDeleted == true ? _dataFilter.Disable<ISoftDelete>() : _dataFilter.Enable<ISoftDelete>())
            {
                var locationRepository = await _locationRepository.GetQueryableAsync();

                var result = (from vt in locationRepository
                              where vt.Id == id
                              select new LocationDto
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

        [Authorize(ProductServicePermissions.Location.List)]
        public async Task<List<LocationDto>> GetListAsync(bool isDeleted)
        {
            using (isDeleted == true ? _dataFilter.Disable<ISoftDelete>() : _dataFilter.Enable<ISoftDelete>())
            {
                var locationRepository = await _locationRepository.GetQueryableAsync();

                var result = (from vt in locationRepository
                              select new LocationDto
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

        //public async Task<List<LocationDto>> GetWithDeletedListAsync()
        //{
        //    using (_dataFilter.Disable<ISoftDelete>())
        //    {
        //        var result = (from vt in _locationRepository
        //                      select new LocationDto
        //                      {
        //                          Name = vt.Name,
        //                          Id = vt.Id,
        //                          ParentId = vt.ParentId,
        //                          IsDeleted = vt.IsDeleted
        //                      }).OrderBy(o => o.Name).ToList();

        //        return await Task.FromResult(result);
        //    }
        //}
    }
}
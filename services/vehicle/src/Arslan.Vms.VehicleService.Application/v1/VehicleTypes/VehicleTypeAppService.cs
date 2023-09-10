using Arslan.Vms.VehicleService.Localization;
using Arslan.Vms.VehicleService.Permissions;
using Arslan.Vms.VehicleService.v1.VehicleTypes.Dtos;
using Arslan.Vms.VehicleService.Vehicles.VehicleTypes;
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

namespace Arslan.Vms.VehicleService.v1.VehicleTypes
{
    [Authorize(VehicleServicePermissions.VehicleType.Default)]
    public class VehicleTypeAppService : VehicleServiceAppService, IVehicleTypeAppService
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IDataFilter _dataFilter;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IStringLocalizer<VehicleServiceResource> _localizer;

        public VehicleTypeAppService(
            IVehicleTypeRepository vehicleTypeRepository,
            IGuidGenerator guidGenerator,
            IDataFilter dataFilter,
            ICurrentTenant currentTenant)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
            _currentTenant = currentTenant;
            _dataFilter = dataFilter;
            _guidGenerator = guidGenerator;
        }

        [Authorize(VehicleServicePermissions.VehicleType.Create)]
        public async Task<VehicleTypeDto> CreateAsync(CreateVehicleTypeDto input)
        {
            //aynı isimden iki kayıt olamaz (pasif kayıtlar da dahil)
            //boş isim olamaz
            //

            using (_dataFilter.Disable<ISoftDelete>())
            {
                var categories = await _vehicleTypeRepository.ToListAsync();

                if (categories.Any(a => a.Name == input.Name))
                {
                    throw new UserFriendlyException(_localizer["Error:VehicleTypeShouldBeUnique"].Value);
                };

                if (string.IsNullOrEmpty(input.Name))
                {
                    throw new UserFriendlyException(_localizer["Error:VehicleTypeShouldNotBeEmpty"].Value);
                };

                var level = 1;
                if (input.ParentId != null)
                {
                    var gg = categories.FirstOrDefault(f => f.Id == input.ParentId);
                    level = gg.Level + 1;
                }

                var data = new VehicleType(_guidGenerator.Create(), _currentTenant.Id, input.ParentId, input.Name, level);
                await _vehicleTypeRepository.InsertAsync(data, true);

                return ObjectMapper.Map<VehicleType, VehicleTypeDto>(data);
            }
        }

        [Authorize(VehicleServicePermissions.VehicleType.Update)]
        public async Task<VehicleTypeDto> UpdateAsync(Guid id, UpdateVehicleTypeDto input)
        {
            //aynı isimden iki kayıt olamaz (pasif kayıtlar da dahil)
            //boş isim olamaz

            using (_dataFilter.Disable<ISoftDelete>())
            {
                var categories = await _vehicleTypeRepository.ToListAsync();

                if (categories.Any(a => a.Id != id && a.Name == input.Name))
                {
                    throw new UserFriendlyException(_localizer["Error:VehicleTypeShouldBeUnique"].Value);
                };

                if (string.IsNullOrEmpty(input.Name))
                {
                    throw new UserFriendlyException(_localizer["Error:VehicleTypeShouldNotBeEmpty"].Value);
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
                await _vehicleTypeRepository.UpdateAsync(dbData, true);

                return ObjectMapper.Map<VehicleType, VehicleTypeDto>(dbData);

            }
        }

        [Authorize(VehicleServicePermissions.VehicleType.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            //son kayıt silinemez
            //alt kayıtları bulunan bir sonraki yere aktarılır
            //alt kategori olan silinemez

            using (_dataFilter.Disable<ISoftDelete>())
            {

                var categories = await _vehicleTypeRepository.ToListAsync();
                var category = categories.FirstOrDefault(f => f.Id == id);

                //son kayıt silinemez
                if (categories.Where(w => w.IsDeleted == false).Count() == 1)
                {
                    throw new UserFriendlyException(_localizer["Error:VehicleTypeLeastOne"].Value);
                };

                //alt kategorisi olan silinemez
                if (categories.Any(a => a.IsDeleted == false && a.ParentId == id))
                {
                    throw new UserFriendlyException(_localizer["Error:VehicleTypeCannotDeleteSubVehicleType"].Value);
                };

                ////kayıtları olan kategory parenta aktarılır
                //if (category.ParentId != null)
                //{
                //    var products = _productRepository.Where(w => w.ProductVehicleTypeId == id).ToList();
                //    foreach (var product in products)
                //    {
                //        product.SetVehicleType(category.ParentId.Value);
                //        await _productRepository.UpdateAsync(product);
                //    }
                //}
                //else
                //{
                //    var products = _productRepository.Where(w => w.ProductVehicleTypeId == id).ToList();
                //    var dbVehicleTypeData = categories.Where(w => w.IsDeleted == false).FirstOrDefault();
                //    foreach (var product in products)
                //    {
                //        product.SetVehicleType(dbVehicleTypeData.Id);
                //        await _productRepository.UpdateAsync(product);
                //    }
                //}

                await _vehicleTypeRepository.DeleteAsync(d => d.Id == id);
            }
        }

        [Authorize(VehicleServicePermissions.VehicleType.Undo)]
        public async Task UndoAsync(Guid id)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var dbData = await _vehicleTypeRepository.FirstOrDefaultAsync(f => f.Id == id);
                dbData.IsDeleted = false;
                await _vehicleTypeRepository.UpdateAsync(dbData, true);

                //parent silinmiş ise undo yap
                if (dbData.ParentId != null)
                {
                    var parentVehicleType = await _vehicleTypeRepository.FirstOrDefaultAsync(f => f.Id == dbData.ParentId);

                    if (parentVehicleType.IsDeleted == true)
                    {
                        parentVehicleType.IsDeleted = false;
                        await _vehicleTypeRepository.UpdateAsync(parentVehicleType, true);
                    }
                }
            }
        }


        public async Task<VehicleTypeDto> GetAsync(Guid id, bool isDeleted)
        {
            using (isDeleted == true ? _dataFilter.Disable<ISoftDelete>() : _dataFilter.Enable<ISoftDelete>())
            {
                var vehicleTypeRepository = await _vehicleTypeRepository.GetQueryableAsync();

                var result = (from vt in vehicleTypeRepository
                              where vt.Id == id
                              select new VehicleTypeDto
                              {
                                  Name = vt.Name,
                                  Id = vt.Id,
                                  ParentId = vt.ParentId,
                                  Level = vt.Level,
                                  IsDeleted = vt.IsDeleted
                              }).OrderBy(o => o.Name).FirstOrDefault();

                return await Task.FromResult(result);
            }
        }

        [Authorize(VehicleServicePermissions.VehicleType.List)]
        public async Task<List<VehicleTypeDto>> GetListAsync(bool isDeleted)
        {
            using (isDeleted == true ? _dataFilter.Disable<ISoftDelete>() : _dataFilter.Enable<ISoftDelete>())
            {
                var vehicleTypeRepository = await _vehicleTypeRepository.GetQueryableAsync();

                var result = (from vt in vehicleTypeRepository
                              select new VehicleTypeDto
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

        public async Task<List<BrandDto>> GetBrandListAsync()
        {
            var vehicleTypeRepository = await _vehicleTypeRepository.GetQueryableAsync();

            var result = vehicleTypeRepository.Where(w => w.Level == 2).Select(s => new BrandDto
            {
                Id = s.Id,
                Name = s.Name
            }).OrderBy(o => o.Name).ToList();

            return await Task.FromResult(result);
        }

        public async Task<List<ModelDto>> GetModelsByBrandAsync(Guid brandId)
        {
            var vehicleTypeRepository = await _vehicleTypeRepository.GetQueryableAsync();

            var result = vehicleTypeRepository.Where(w => w.ParentId == brandId).Select(s => new ModelDto
            {
                BrandId = s.ParentId,
                Id = s.Id,
                Name = s.Name
            }).OrderBy(o => o.Name).ToList();

            return await Task.FromResult(result);
        }

        public async Task<List<ModelDto>> GetAllModelsAsync()
        {
            var vehicleTypeRepository = await _vehicleTypeRepository.GetQueryableAsync();

            var result = vehicleTypeRepository.Where(w => w.Level == 3).Select(s => new ModelDto
            {
                BrandId = s.ParentId,
                Id = s.Id,
                Name = s.Name
            }).OrderBy(o => o.Name).ToList();

            return await Task.FromResult(result);
        }
    }
}
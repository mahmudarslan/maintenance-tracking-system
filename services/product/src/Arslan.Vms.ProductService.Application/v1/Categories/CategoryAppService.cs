using Arslan.Vms.ProductService.Categories;
using Arslan.Vms.ProductService.Localization;
using Arslan.Vms.ProductService.Permissions;
using Arslan.Vms.ProductService.Products;
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

namespace Arslan.Vms.ProductService.v1.Categories
{
    [Authorize(ProductServicePermissions.Category.Default)]
    public class CategoryAppService : ProductServiceAppService, ICategoryAppService
    {
        #region Fields
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly ICategoryRepository _productCategoryRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IStringLocalizer<ProductServiceResource> _localizer;
        private readonly IDataFilter _dataFilter;
        #endregion

        #region Ctor
        public CategoryAppService(
      ICategoryRepository productCategoryRepository,
      IRepository<Product, Guid> productRepository,
      ICurrentTenant currentTenant,
      IGuidGenerator guidGenerator,
      IDataFilter dataFilter,
      IStringLocalizer<ProductServiceResource> localizer
    )
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;
            _localizer = localizer;
            _dataFilter = dataFilter;
        }
        #endregion

        [Authorize(ProductServicePermissions.Category.Create)]
        public async Task<CategoryDto> CreateAsync(CreateCategoryDto input)
        {
            //aynı isimden iki kayıt olamaz (pasif kayıtlar da dahil)
            //boş isim olamaz
            //

            using (_dataFilter.Disable<ISoftDelete>())
            {
                var categories = await _productCategoryRepository.ToListAsync();

                if (categories.Any(a => a.Name == input.Name))
                {
                    throw new UserFriendlyException(_localizer["Error:CategoryShouldBeUnique"].Value);
                };

                if (string.IsNullOrEmpty(input.Name))
                {
                    throw new UserFriendlyException(_localizer["Error:CategoryShouldNotBeEmpty"].Value);
                };

                var level = 1;
                if (input.ParentId != null)
                {
                    var gg = categories.FirstOrDefault(f => f.Id == input.ParentId);
                    level = gg.Level + 1;
                }

                var data = new Category(_guidGenerator.Create(), _currentTenant.Id, input.ParentId, input.Name, level);
                await _productCategoryRepository.InsertAsync(data, true);

                return ObjectMapper.Map<Category, CategoryDto>(data);
            }
        }

        [Authorize(ProductServicePermissions.Category.Update)]
        public async Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto input)
        {
            //aynı isimden iki kayıt olamaz (pasif kayıtlar da dahil)
            //boş isim olamaz

            using (_dataFilter.Disable<ISoftDelete>())
            {
                var categories = await _productCategoryRepository.ToListAsync();

                if (categories.Any(a => a.Id != id && a.Name == input.Name))
                {
                    throw new UserFriendlyException(_localizer["Error:CategoryShouldBeUnique"].Value);
                };

                if (string.IsNullOrEmpty(input.Name))
                {
                    throw new UserFriendlyException(_localizer["Error:CategoryShouldNotBeEmpty"].Value);
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
                await _productCategoryRepository.UpdateAsync(dbData, true);

                return ObjectMapper.Map<Category, CategoryDto>(dbData);

            }
        }

        [Authorize(ProductServicePermissions.Category.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            //son kayıt silinemez
            //alt kayıtları bulunan bir sonraki yere aktarılır
            //alt kategori olan silinemez

            using (_dataFilter.Disable<ISoftDelete>())
            {
                var productRepository = await _productRepository.GetQueryableAsync();
                var categories = await _productCategoryRepository.ToListAsync();
                var category = categories.FirstOrDefault(f => f.Id == id);

                //son kayıt silinemez
                if (categories.Where(w => w.IsDeleted == false).Count() == 1)
                {
                    throw new UserFriendlyException(_localizer["Error:CategoryLeastOne"].Value);
                };

                //alt kategorisi olan silinemez
                if (categories.Any(a => a.IsDeleted == false && a.ParentId == id))
                {
                    throw new UserFriendlyException(_localizer["Error:CategoryCannotDeleteSubCategory"].Value);
                };

                //kayıtları olan kategory parenta aktarılır
                if (category.ParentId != null)
                {
                    var products = productRepository.Where(w => w.ProductCategoryId == id).ToList();
                    foreach (var product in products)
                    {
                        product.SetCategory(category.ParentId.Value);
                        await _productRepository.UpdateAsync(product);
                    }
                }
                else
                {
                    var products = productRepository.Where(w => w.ProductCategoryId == id).ToList();
                    var dbCategoryData = categories.Where(w => w.IsDeleted == false).FirstOrDefault();
                    foreach (var product in products)
                    {
                        product.SetCategory(dbCategoryData.Id);
                        await _productRepository.UpdateAsync(product);
                    }
                }

                await _productCategoryRepository.DeleteAsync(d => d.Id == id);
            }
        }

        [Authorize(ProductServicePermissions.Category.Undo)]
        public async Task UndoAsync(Guid id)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var dbData = await _productCategoryRepository.FirstOrDefaultAsync(f => f.Id == id);
                dbData.IsDeleted = false;
                await _productCategoryRepository.UpdateAsync(dbData, true);

                //parent silinmiş ise undo yap
                if (dbData.ParentId != null)
                {
                    var parentCategory = await _productCategoryRepository.FirstOrDefaultAsync(f => f.Id == dbData.ParentId);

                    if (parentCategory.IsDeleted == true)
                    {
                        parentCategory.IsDeleted = false;
                        await _productCategoryRepository.UpdateAsync(parentCategory, true);
                    }
                }
            }
        }


        public async Task<CategoryDto> GetAsync(Guid id, bool isDeleted)
        {
            using (isDeleted == true ? _dataFilter.Disable<ISoftDelete>() : _dataFilter.Enable<ISoftDelete>())
            {
                var productCategoryRepository = await _productCategoryRepository.GetQueryableAsync();

                var result = (from vt in productCategoryRepository
                              where vt.Id == id
                              select new CategoryDto
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

        [Authorize(ProductServicePermissions.Category.List)]
        public async Task<List<CategoryDto>> GetListAsync(bool isDeleted)
        {
            using (isDeleted == true ? _dataFilter.Disable<ISoftDelete>() : _dataFilter.Enable<ISoftDelete>())
            {
                var productCategoryRepository = await _productCategoryRepository.GetQueryableAsync();

                var result = (from vt in productCategoryRepository
                              select new CategoryDto
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

        //public async Task<List<CategoryDto>> GetWithDeletedListAsync()
        //{
        //    using (_dataFilter.Disable<ISoftDelete>())
        //    {
        //        var result = (from vt in _productCategoryRepository
        //                      select new CategoryDto
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
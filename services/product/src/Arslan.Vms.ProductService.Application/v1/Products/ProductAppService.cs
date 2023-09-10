using Arslan.Vms.ProductService.Categories;
using Arslan.Vms.ProductService.Files;
using Arslan.Vms.ProductService.Localization;
using Arslan.Vms.ProductService.Permissions;
using Arslan.Vms.ProductService.Pricing;
using Arslan.Vms.ProductService.Products;
using Arslan.Vms.ProductService.Products.Versions;
using Arslan.Vms.ProductService.v1.Files.Dtos;
using Arslan.Vms.ProductService.v1.Products.Dtos;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
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

namespace Arslan.Vms.ProductService.v1.Products
{
    [Authorize(ProductServicePermissions.Product.Default)]
    public class ProductAppService : ProductServiceAppService, IProductAppService
    {
        #region Fields
        private readonly IProductRepository _productRepository;
        private readonly ProductManager _productManager;
        private readonly IProductPriceRepository _productPriceRepository;
        private readonly IRepository<ProductVersion, Guid> _productVersionRepository;
        private readonly IRepository<ProductPriceVersion, Guid> _productPriceVersionRepository;
        private readonly IRepository<Category, Guid> _productCategoryRepository;
        private readonly IRepository<PricingScheme, Guid> _pricingSchemeRepository;
        private readonly IRepository<FileAttachment, Guid> _fileAttachmentRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IDataFilter _dataFilter;
        private readonly IStringLocalizer<ProductServiceResource> _localizer;
        #endregion

        #region Ctor
        public ProductAppService(IProductRepository productRepository,
       IProductPriceRepository productPriceRepository,
       IRepository<ProductVersion, Guid> productVersionRepository,
       IRepository<ProductPriceVersion, Guid> productPriceVersionRepository,
       IRepository<PricingScheme, Guid> pricingSchemeRepository,
       IRepository<Category, Guid> productCategoryRepository,
       IRepository<FileAttachment, Guid> fileAttachmentRepository,
       ProductManager productManager,
       ICurrentTenant currentTenant,
       IGuidGenerator guidGenerator,
       IStringLocalizer<ProductServiceResource> localizer,
       IDataFilter dataFilter
    )
        {
            _productRepository = productRepository;
            _productVersionRepository = productVersionRepository;
            _productPriceVersionRepository = productPriceVersionRepository;
            _productPriceRepository = productPriceRepository;
            _pricingSchemeRepository = pricingSchemeRepository;
            _productCategoryRepository = productCategoryRepository;
            _productManager = productManager;
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;
            _localizer = localizer;
            _fileAttachmentRepository = fileAttachmentRepository;
            _dataFilter = dataFilter;
        }
        #endregion

        #region Methods

        [Authorize(ProductServicePermissions.Product.Create)]
        public async Task<ProductDto> CreateAsync(CreateProductDto input)
        {
            var fileAttachmentRepository = await _fileAttachmentRepository.GetQueryableAsync();

            var product = new Product(_guidGenerator.Create(), _currentTenant.Id, input.ProductType, input.Name, input.ProductCategoryId)
            {
                DefaultLocationId = input.DefaultLocationId,
                Description = input.Description,
                Remarks = input.Remarks,
                Barcode = input.Barcode,
                ReorderPoint = input.ReorderPoint,
                UnitCost = input.UnitCost
            };

            foreach (var item in input.Prices ?? Enumerable.Empty<CreateUpdateProductPriceDto>())
            {
                product.AddPrice(_guidGenerator.Create(), item.PricingSchemeId, item.UnitPrice, item.ItemPriceType, item.FixedMarkup);
            }

            foreach (var item in input.Taxes ?? Enumerable.Empty<CreateUpdateProductTaxDto>())
            {
                product.AddTax(_guidGenerator.Create(), item.TaxingSchemeId, item.TaxCodeId);
            }

            var files = fileAttachmentRepository.Where(w => w.DownloadGuid == input.FakeId).ToList();
            foreach (var file in files ?? Enumerable.Empty<FileAttachment>())
            {
                product.AddAttachment(_guidGenerator.Create(), file.Id);
            }

            await _productManager.CreateAsync(product);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        [Authorize(ProductServicePermissions.Product.Update)]
        public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto input)
        {
            var fileAttachmentRepository = await _fileAttachmentRepository.GetQueryableAsync();

            var product = await _productRepository.GetAsync(f => f.Id == id);
            product.SetProduct(input.ProductType, input.Name, input.ProductCategoryId);
            product.DefaultLocationId = input.DefaultLocationId;
            product.Description = input.Description;
            product.Remarks = input.Remarks;
            product.Barcode = input.Barcode;
            product.ReorderPoint = input.ReorderPoint;
            product.UnitCost = input.UnitCost;

            //Update Price Lines
            foreach (var inputLine in input.Prices ?? Enumerable.Empty<CreateUpdateProductPriceDto>())
            {
                //Create Line
                if (!product.Prices.Any(a => a.Id == inputLine.Id))
                {
                    product.AddPrice(_guidGenerator.Create(), inputLine.PricingSchemeId, inputLine.UnitPrice, inputLine.ItemPriceType, inputLine.FixedMarkup);
                }
                //Update Line
                else
                {
                    product.SetPrice(inputLine.Id, inputLine.PricingSchemeId, inputLine.UnitPrice, inputLine.ItemPriceType, inputLine.FixedMarkup);
                }
            }

            #region  Attachment
            //Insert Attachment Files
            var files = fileAttachmentRepository.Where(w => w.DownloadGuid == input.FakeId).ToList();

            foreach (var file in files ?? Enumerable.Empty<FileAttachment>())
            {
                product.AddAttachment(_guidGenerator.Create(), file.Id);
            }

            //Update Attachment Files
            foreach (var item in input.Files?.Where(w => w.IsDeleted == true) ?? Enumerable.Empty<FileAttachmentDto>())
            {
                var file = fileAttachmentRepository.Where(w => w.Id == item.Id).FirstOrDefault();
                file.IsDeleted = true;
                await _fileAttachmentRepository.UpdateAsync(file);
            }
            #endregion

            await _productManager.UpdateAsync(product);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        [Authorize(ProductServicePermissions.Product.Delete)]
        public async Task DeleteAsync(string key)
        {
            await _productManager.DeleteAsync(key);
        }

        [Authorize(ProductServicePermissions.Product.Undo)]
        public async Task UndoAsync(Guid id)
        {
            await _productManager.UndoAsync(id);
        }

        public async Task<ProductDto> GetAsync(Guid id, bool isDeleted)
        {
            var product = new ProductDto();

            var productRepository = await _productRepository.GetQueryableAsync();
            var productPriceRepository = await _productPriceRepository.GetQueryableAsync();
            var fileAttachmentRepository = await _fileAttachmentRepository.GetQueryableAsync();

            using (isDeleted == true ? _dataFilter.Disable<ISoftDelete>() : _dataFilter.Enable<ISoftDelete>())
            {


                product =
                    (from p in productRepository
                     where p.Id == id
                     select new ProductDto
                     {
                         Barcode = p.Barcode,
                         Name = p.Name,
                         Id = p.Id,
                         UnitCost = p.UnitCost,
                         DefaultLocationId = p.DefaultLocationId,
                         Description = p.Description,
                         Remarks = p.Remarks,
                         ProductCategoryId = p.ProductCategoryId,
                         ProductType = p.ProductType,
                         ReorderPoint = p.ReorderPoint,
                         IsDeleted = p.IsDeleted,
                         Version = p.Version
                     }).FirstOrDefault();
            }

            var prices = productPriceRepository.Where(w => w.ProductId == product.Id).ToList();

            product.Prices = ObjectMapper.Map<List<ProductPrice>, List<ProductPriceDto>>(prices);

            #region Files

            product.Attachments = (from product1 in productRepository
                                   from file in fileAttachmentRepository
                                   from rfile in product1.Attachments.Where(w => w.FileAttachmentId == file.Id)
                                   where rfile.ProductId == product.Id
                                   select new FileAttachmentDto
                                   {
                                       Id = file.Id,
                                       FileName = file.FileName,
                                       Extension = file.Extension,
                                       DownloadUrl = $"{file.DownloadUrl}/{file.Id}"
                                   }
                ).ToList();

            #endregion

            return await Task.FromResult(product);
        }

        public async Task<ProductDto> GetVersionAsync(Guid id, int version)
        {
            var product = new ProductDto();

            var productRepository = await _productRepository.GetQueryableAsync();
            var productVersionRepository = await _productVersionRepository.GetQueryableAsync();
            var productPriceRepository = await _productPriceRepository.GetQueryableAsync();
            var productPriceVersionRepository = await _productPriceVersionRepository.GetQueryableAsync();
            var pricingSchemeRepository = await _pricingSchemeRepository.GetQueryableAsync();
            var productCategoryRepository = await _productCategoryRepository.GetQueryableAsync();
            var fileAttachmentRepository = await _fileAttachmentRepository.GetQueryableAsync();

            //old data
            var proVersion = productVersionRepository.Where(w => w.ProductId == id && w.Version == version).FirstOrDefault();
            product = ObjectMapper.Map<ProductVersion, ProductDto>(proVersion);

            if (product != null)
            {
                var prices = productPriceVersionRepository.Where(w => w.ProductId == product.Id && w.Version == version).ToList();
                product.Prices = ObjectMapper.Map<List<ProductPriceVersion>, List<ProductPriceDto>>(prices);
            }

            //current data
            if (product == null)
            {
                var pro = productRepository.Where(w => w.Id == id && w.Version == version).FirstOrDefault();
                product = ObjectMapper.Map<Product, ProductDto>(pro);

                var prices = productPriceRepository.Where(w => w.ProductId == product.Id && w.Version == version).ToList();
                product.Prices = ObjectMapper.Map<List<ProductPrice>, List<ProductPriceDto>>(prices);
            }


            #region Files
            product.Attachments = (from product1 in productRepository
                                   from file in fileAttachmentRepository
                                   from rfile in product1.Attachments.Where(w => w.FileAttachmentId == file.Id)
                                   where rfile.ProductId == product.Id
                                   select new FileAttachmentDto
                                   {
                                       Id = file.Id,
                                       FileName = file.FileName,
                                       Extension = file.Extension,
                                       DownloadUrl = $"{file.DownloadUrl}/{file.Id}"
                                   }
                ).ToList();
            #endregion

            return await Task.FromResult(product);
        }

        [Authorize(ProductServicePermissions.Product.List)]
        public async Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            loadOptions.PrimaryKey = new[] { "Id" };

            using (_dataFilter.Disable<ISoftDelete>())
            {
                var productRepository = await _productRepository.GetQueryableAsync();
                var productPriceRepository = await _productPriceRepository.GetQueryableAsync();
                var pricingSchemeRepository = await _pricingSchemeRepository.GetQueryableAsync();
                var productCategoryRepository = await _productCategoryRepository.GetQueryableAsync();

                var ls = from p in productRepository
                         from price in productPriceRepository.Where(w => w.ProductId == p.Id)
                         from priceSheme in pricingSchemeRepository.Where(w => w.Id == price.PricingSchemeId && w.Name == "Normal Price")
                         from category in productCategoryRepository.Where(w => w.Id == p.ProductCategoryId)
                         select new ProductListDto
                         {
                             Id = p.Id,
                             CreationTime = p.CreationTime,
                             Name = p.Name,
                             ProductType = p.ProductType,
                             ProductCategoryId = p.ProductCategoryId,
                             CategoryName = category.Name,
                             NormalPrice = price.UnitPrice,
                             IsDeleted = p.IsDeleted,
                         };

                return await DataSourceLoader.LoadAsync(ls, loadOptions);
            }
        }

        public async Task<List<ProductItemDto>> GetAllServiceAsync()
        {
            var productRepository = await _productRepository.GetQueryableAsync();
            var productPriceRepository = await _productPriceRepository.GetQueryableAsync();
            var pricingSchemeRepository = await _pricingSchemeRepository.GetQueryableAsync();

            var result = (from product in productRepository.Where(w => w.ProductType == ProductType.Service)
                          from price in productPriceRepository.Where(w => w.ProductId == product.Id)
                          from priceSheme in pricingSchemeRepository.Where(w => w.Id == price.PricingSchemeId && w.Name == "Normal Price")
                          select new ProductItemDto
                          {
                              Barcode = product.Barcode,
                              Name = product.Name,
                              IsDeleted = product.IsDeleted,
                              Id = product.Id,
                              UnitCost = product.UnitCost,
                              DefaultLocationId = product.DefaultLocationId,
                              NormalPrice = price.UnitPrice,
                              Description = product.Description,
                              Remarks = product.Remarks,
                              ProductCategoryId = product.ProductCategoryId,
                              ProductType = product.ProductType,
                          }).ToList();

            return await Task.FromResult(result);
        }

        public async Task<List<ProductItemDto>> GetAllStockedProductAsync()
        {
            var productRepository = await _productRepository.GetQueryableAsync();
            var productPriceRepository = await _productPriceRepository.GetQueryableAsync();
            var pricingSchemeRepository = await _pricingSchemeRepository.GetQueryableAsync();

            var result = (from product in productRepository.Where(w => w.ProductType == ProductType.StockedProduct)
                          from price in productPriceRepository.Where(w => w.ProductId == product.Id)
                          from priceSheme in pricingSchemeRepository.Where(w => w.Id == price.PricingSchemeId && w.Name == "Normal Price")
                          select new ProductItemDto
                          {
                              Barcode = product.Barcode,
                              Name = product.Name,
                              IsDeleted = product.IsDeleted,
                              Id = product.Id,
                              UnitCost = product.UnitCost,
                              DefaultLocationId = product.DefaultLocationId,
                              NormalPrice = price.UnitPrice,
                              Description = product.Description,
                              Remarks = product.Remarks,
                              ProductCategoryId = product.ProductCategoryId,
                              ProductType = product.ProductType,
                          }).ToList();

            return await Task.FromResult(result);
        }

        public async Task<List<ProductItemDto>> GetAllProductAsync()
        {
            var productRepository = await _productRepository.GetQueryableAsync();
            var productPriceRepository = await _productPriceRepository.GetQueryableAsync();
            var pricingSchemeRepository = await _pricingSchemeRepository.GetQueryableAsync();

            var result = (from product in productRepository.Where(w => w.ProductType == ProductType.StockedProduct || w.ProductType == ProductType.NonStockedProduct)
                          from price in productPriceRepository.Where(w => w.ProductId == product.Id)
                          from priceSheme in pricingSchemeRepository.Where(w => w.Id == price.PricingSchemeId && w.Name == "Normal Price")
                          select new ProductItemDto
                          {
                              Barcode = product.Barcode,
                              Name = product.Name,
                              IsDeleted = product.IsDeleted,
                              Id = product.Id,
                              UnitCost = product.UnitCost,
                              DefaultLocationId = product.DefaultLocationId,
                              NormalPrice = price.UnitPrice,
                              Description = product.Description,
                              Remarks = product.Remarks,
                              ProductCategoryId = product.ProductCategoryId,
                              ProductType = product.ProductType,
                          }).ToList();

            return await Task.FromResult(result);
        }

        public async Task<List<ProductTypeDto>> GetProductTypeListAsync()
        {
            var data = new List<ProductTypeDto>();

            foreach (var item in Enum.GetValues(typeof(ProductType)))
            {
                data.Add(new ProductTypeDto { Id = (int)item, ProductTypeName = _localizer[item.ToString()].Value });
            }

            return await Task.FromResult(data);
        }

        #endregion
    }
}
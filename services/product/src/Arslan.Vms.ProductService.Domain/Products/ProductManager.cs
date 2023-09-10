using Arslan.Vms.ProductService.DocumentNoFormats;
using Arslan.Vms.ProductService.Localization;
using Arslan.Vms.ProductService.Products.Versions;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;

namespace Arslan.Vms.ProductService.Products
{
    public class ProductManager : DomainService
    {
        #region Fields
        private readonly IProductRepository _productRepository;
        private readonly IRepository<ProductVersion, Guid> _productVersionRepository;
        private readonly IRepository<ProductPriceVersion, Guid> _priceVersionRepository;
        private readonly DocNoFormatManager _docNoFormatManager;
        private readonly IStringLocalizer<ProductServiceResource> _localizer;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IDataFilter _dataFilter;
        private readonly IObjectMapper _objectMapper;
        #endregion

        #region Ctor
        public ProductManager(
     IProductRepository productRepository,
     IRepository<ProductVersion, Guid> productVersionRepository,
     IRepository<ProductPriceVersion, Guid> priceVersionRepository,
     ICurrentTenant currentTenant,
     IDataFilter dataFilter,
     IGuidGenerator guidGenerator,
     IObjectMapper objectMapper,
     IStringLocalizer<ProductServiceResource> localizer,
     DocNoFormatManager docNoFormatManager
    )
        {
            _productRepository = productRepository;
            _productVersionRepository = productVersionRepository;
            _priceVersionRepository = priceVersionRepository;
            _docNoFormatManager = docNoFormatManager;
            _currentTenant = currentTenant;
            _dataFilter = dataFilter;
            _guidGenerator = guidGenerator;
            _localizer = localizer;
            _objectMapper = objectMapper;
        }
        #endregion

        public async Task CreateAsync(Product product)
        {
            await _productRepository.InsertAsync(product, true);
        }

        public async Task UpdateAsync(Product product)
        {
            product.IncreaseVersion();

            await Version(product.Id);

            await _productRepository.UpdateAsync(product, true);
        }

        public async Task DeleteAsync(string key)
        {
            var product = await _productRepository.FirstOrDefaultAsync(f => f.Id == Guid.Parse(key));
            product.SetActiveted(false);
            await _productRepository.DeleteAsync(product, true);
        }

        public async Task UndoAsync(Guid id)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var product = await _productRepository.FirstOrDefaultAsync(f => f.Id == id);
                product.SetActiveted(true);
                await _productRepository.UpdateAsync(product, true);
            }
        }

        async Task Version(Guid id)
        {
            var dbData = await _productRepository.GetAsNoTracking(id);

            var oldData = _objectMapper.Map<Product, ProductVersion>(dbData);
            await _productVersionRepository.InsertAsync(oldData);

            foreach (var item in dbData.Prices)
            {
                var oldPrice = _objectMapper.Map<ProductPrice, ProductPriceVersion>(item);
                await _priceVersionRepository.InsertAsync(oldPrice);
            }
        }
    }
}
using Arslan.Vms.ProductService.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.ObjectMapping;

namespace Arslan.Vms.ProductService.Products
{
    public class ProductPriceRepository : EfCoreRepository<IProductServiceDbContext, ProductPrice, Guid>, IProductPriceRepository
    {
        //private readonly IRepository<ProductPriceVersion, Guid> _productPriceVersionRepository;
        private readonly IObjectMapper _objectMapper;
        IDbContextProvider<IProductServiceDbContext> _dbContextProvider;

        public ProductPriceRepository(IDbContextProvider<IProductServiceDbContext> dbContextProvider,
            //IRepository<ProductPriceVersion, Guid> productPriceVersionRepository,
            IObjectMapper objectMapper) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            //_productPriceVersionRepository = productPriceVersionRepository;
            _objectMapper = objectMapper;
        }

        public async override Task<ProductPrice> InsertAsync(ProductPrice entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var data = await base.InsertAsync(entity, autoSave, cancellationToken);
            return data;
        }

        //public async override Task<ProductPrice> UpdateAsync(ProductPrice entity, bool autoSave = false, CancellationToken cancellationToken = default)
        //{
        //    await Version(entity.Id);
        //    var data = await base.UpdateAsync(entity, autoSave, cancellationToken);
        //    return data;
        //}

        //async Task Version(Guid Id)
        //{
        //    var data = _dbContextProvider.GetDbContext().ProductPrice.AsNoTracking().FirstOrDefault(f => f.Id == Id);
        //    var oldData = _objectMapper.Map<ProductPrice, ProductPriceVersion>(data);
        //    await _productPriceVersionRepository.InsertAsync(oldData, true);
        //}
    }
}
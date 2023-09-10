using Arslan.Vms.ProductService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.ObjectMapping;

namespace Arslan.Vms.ProductService.Products
{
    public class ProductRepository : EfCoreRepository<IProductServiceDbContext, Product, Guid>, IProductRepository
    {
        private readonly IObjectMapper _objectMapper;
        IDbContextProvider<IProductServiceDbContext> _dbContextProvider;

        public ProductRepository(IDbContextProvider<IProductServiceDbContext> dbContextProvider,
            IObjectMapper objectMapper) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            _objectMapper = objectMapper;
        }

        public override IQueryable<Product> WithDetails()
        {
            return base.WithDetails(w => w.Prices, a => a.TaxCodes);
        }

        public async Task<Product> GetAsNoTracking(Guid id)
        {
            var data = _dbContextProvider.GetDbContext().Product.AsNoTracking().FirstOrDefault(f => f.Id == id);
            data.SetPrices(_dbContextProvider.GetDbContext().ProductPrice.AsNoTracking().Where(w => w.ProductId == id).ToList());
            return data;
        }
    }
}
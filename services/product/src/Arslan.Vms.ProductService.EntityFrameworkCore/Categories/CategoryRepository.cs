using Arslan.Vms.ProductService.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.ObjectMapping;

namespace Arslan.Vms.ProductService.Categories
{
    public class CategoryRepository : EfCoreRepository<IProductServiceDbContext, Category, Guid>, ICategoryRepository
    {
        private readonly IObjectMapper _objectMapper;

        IDbContextProvider<IProductServiceDbContext> _dbContextProvider;

        public CategoryRepository(IDbContextProvider<IProductServiceDbContext> dbContextProvider,
            IObjectMapper objectMapper) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            _objectMapper = objectMapper;
        }

        public async Task AddRangeAsync(List<Category> entityList, CancellationToken cancellationToken = default)
        {
            await base.DbContext.ProductCategory.AddRangeAsync(entityList, cancellationToken);
        }

        public void UpdateRange(List<Category> entityList)
        {
            base.DbContext.ProductCategory.UpdateRange(entityList);
        }

    }
}
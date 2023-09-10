using Arslan.Vms.ProductService.Addresses.AddressTypes;
using Arslan.Vms.ProductService.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.ObjectMapping;

namespace Arslan.Vms.ProductService.AddressTypes
{
    public class AddressTypeRepository : EfCoreRepository<ProductServiceDbContext, AddressType, Guid>, IAddressTypeRepository
    {
        private readonly IObjectMapper _objectMapper;
        IDbContextProvider<ProductServiceDbContext> _dbContextProvider;

        public AddressTypeRepository(IDbContextProvider<ProductServiceDbContext> dbContextProvider,
            IObjectMapper objectMapper) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            _objectMapper = objectMapper;
        }

        public async Task AddRangeAsync(List<AddressType> entityList, CancellationToken cancellationToken = default)
        {
            await base.DbContext.AddressType.AddRangeAsync(entityList, cancellationToken);
        }

        public void UpdateRange(List<AddressType> entityList)
        {
            base.DbContext.AddressType.UpdateRange(entityList);
        }

    }
}
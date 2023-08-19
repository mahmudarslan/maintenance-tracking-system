using Arslan.Vms.IdentityService.Addresses.AddressTypes;
using Arslan.Vms.IdentityService.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.ObjectMapping;

namespace Arslan.Vms.IdentityService.AddressTypes
{
    public class AddressTypeRepository : EfCoreRepository<IdentityServiceDbContext, AddressType, Guid>, IAddressTypeRepository
    {
        private readonly IObjectMapper _objectMapper;
        IDbContextProvider<IdentityServiceDbContext> _dbContextProvider;

        public AddressTypeRepository(IDbContextProvider<IdentityServiceDbContext> dbContextProvider,
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
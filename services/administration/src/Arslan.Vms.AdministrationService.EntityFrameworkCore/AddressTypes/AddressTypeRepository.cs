using Arslan.Vms.AdministrationService.Addresses.AddressTypes;
using Arslan.Vms.AdministrationService.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.ObjectMapping;

namespace Arslan.Vms.AdministrationService.AddressTypes
{
    public class AddressTypeRepository : EfCoreRepository<AdministrationServiceDbContext, AddressType, Guid>, IAddressTypeRepository
    {
        private readonly IObjectMapper _objectMapper;
        IDbContextProvider<AdministrationServiceDbContext> _dbContextProvider;

        public AddressTypeRepository(IDbContextProvider<AdministrationServiceDbContext> dbContextProvider,
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
using Arslan.Vms.ProductService.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.ObjectMapping;

namespace Arslan.Vms.ProductService.Locations
{
    public class LocationRepository : EfCoreRepository<IProductServiceDbContext, Location, Guid>, ILocationRepository
    {
        private readonly IObjectMapper _objectMapper;

        IDbContextProvider<IProductServiceDbContext> _dbContextProvider;

        public LocationRepository(IDbContextProvider<IProductServiceDbContext> dbContextProvider,
            IObjectMapper objectMapper) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            _objectMapper = objectMapper;
        }

        public async Task AddRangeAsync(List<Location> entityList, CancellationToken cancellationToken = default)
        {
            await base.DbContext.Location.AddRangeAsync(entityList, cancellationToken);
        }

        public void UpdateRange(List<Location> entityList)
        {
            base.DbContext.Location.UpdateRange(entityList);
        }

    }
}
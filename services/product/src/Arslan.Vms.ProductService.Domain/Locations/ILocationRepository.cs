using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.ProductService.Locations
{
    public interface ILocationRepository : IRepository<Location, Guid>
    {
        Task AddRangeAsync(List<Location> entityList, CancellationToken cancellationToken = default);
        void UpdateRange(List<Location> entityList);
    }
}
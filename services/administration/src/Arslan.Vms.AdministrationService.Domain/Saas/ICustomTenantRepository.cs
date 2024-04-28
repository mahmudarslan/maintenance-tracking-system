using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TenantManagement;

namespace Arslan.Vms.AdministrationService.Saas;

public interface ICustomTenantRepository : IBasicRepository<Tenant, Guid>
{
    Task<Tenant> GetTenantByHost(string host,
        CancellationToken cancellationToken = default);

    Task<Tenant> FindByNameAsync(
    string name,
    bool includeDetails = true,
    CancellationToken cancellationToken = default);

    Task<List<Tenant>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        string filter = null,
        CancellationToken cancellationToken = default);
}

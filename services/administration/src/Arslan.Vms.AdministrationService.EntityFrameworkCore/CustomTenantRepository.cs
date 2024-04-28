using Arslan.Vms.AdministrationService.EntityFrameworkCore;
using Arslan.Vms.AdministrationService.Saas;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Arslan.Vms.AdministrationService;

public class CustomTenantRepository : EfCoreRepository<AdministrationServiceDbContext, Tenant, Guid>, ICustomTenantRepository
{
    public CustomTenantRepository(IDbContextProvider<AdministrationServiceDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<Tenant> GetTenantByHost(string host, CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync();
        var tenant = context.Tenants.Where(u => EF.Property<string>(u, "Host") == host);
        return await tenant.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<Tenant> FindByNameAsync(
      string name,
      bool includeDetails = true,
      CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .OrderBy(t => t.Id)
            .FirstOrDefaultAsync(t => t.Name == name, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Tenant>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.Name.Contains(filter)
            )
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(Tenant.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.Name.Contains(filter)
            ).CountAsync(cancellationToken: GetCancellationToken(cancellationToken));
    }

    public override async Task<IQueryable<Tenant>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}

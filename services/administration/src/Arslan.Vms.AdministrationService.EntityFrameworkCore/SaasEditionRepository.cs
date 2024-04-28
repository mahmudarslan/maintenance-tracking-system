using Arslan.Vms.AdministrationService.EntityFrameworkCore;
using Arslan.Vms.AdministrationService.Saas;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.AdministrationService;

public class SaasEditionRepository : EfCoreRepository<AdministrationServiceDbContext, SaasEdition, Guid>, ISaasEditionRepository
{
    public SaasEditionRepository(IDbContextProvider<AdministrationServiceDbContext> dbContextProvider) : base(dbContextProvider)
    {

    }

    public async Task<SaasEdition> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
             .FirstOrDefaultAsync(t => t.Name == name, GetCancellationToken(cancellationToken));
    }

    public async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
           .WhereIf(
               !filter.IsNullOrWhiteSpace(),
               u =>
                   u.Name.Contains(filter)
           ).CountAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<SaasEdition>> GetListAsync(string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, string filter = null, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
           .WhereIf(
               !filter.IsNullOrWhiteSpace(),
               u =>
                   u.Name.Contains(filter)
           )
           .OrderBy(o => o.Name)
           .PageBy(skipCount, maxResultCount)
           .ToListAsync(GetCancellationToken(cancellationToken));
    }
}

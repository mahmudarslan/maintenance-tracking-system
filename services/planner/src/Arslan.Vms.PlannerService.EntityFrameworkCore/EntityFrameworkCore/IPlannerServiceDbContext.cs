using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.PlannerService.EntityFrameworkCore;

[ConnectionStringName(PlannerServiceDbProperties.ConnectionStringName)]
public interface IPlannerServiceDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}

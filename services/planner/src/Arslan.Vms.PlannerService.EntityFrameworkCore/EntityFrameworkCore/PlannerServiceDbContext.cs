using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.PlannerService.EntityFrameworkCore;

[ConnectionStringName(PlannerServiceDbProperties.ConnectionStringName)]
public class PlannerServiceDbContext : AbpDbContext<PlannerServiceDbContext>, IPlannerServiceDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public PlannerServiceDbContext(DbContextOptions<PlannerServiceDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigurePlannerService();
    }
}

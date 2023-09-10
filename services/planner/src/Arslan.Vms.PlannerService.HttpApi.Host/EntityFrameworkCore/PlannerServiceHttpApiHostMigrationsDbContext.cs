using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.PlannerService.EntityFrameworkCore;

public class PlannerServiceHttpApiHostMigrationsDbContext : AbpDbContext<PlannerServiceHttpApiHostMigrationsDbContext>
{
    public PlannerServiceHttpApiHostMigrationsDbContext(DbContextOptions<PlannerServiceHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigurePlannerService();
    }
}

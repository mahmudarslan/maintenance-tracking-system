using Arslan.Vms.InventoryService.StockAdjustments;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Arslan.Vms.InventoryService.EntityFrameworkCore;

[DependsOn(
    typeof(InventoryServiceDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class InventoryServiceEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        InventoryServiceEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<InventoryServiceDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
            options.AddDefaultRepositories<IInventoryServiceDbContext>();
            options.AddRepository<StockAdjustment, StockAdjustmentRepository>();
        });

        Configure<AbpDbContextOptions>(options =>
        {
            /* The main point to change your DBMS.
             * See also OrderingServiceMigrationsDbContextFactory for EF Core tooling. */
            options.UseSqlServer(b =>
            {
                b.MigrationsHistoryTable("__InventoryService_Migrations");
            });
        });
    }
}

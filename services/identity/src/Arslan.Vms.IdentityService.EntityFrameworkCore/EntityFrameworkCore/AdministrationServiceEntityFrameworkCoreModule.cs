using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Arslan.Vms.IdentityService.EntityFrameworkCore;

[DependsOn(
    typeof(IdentityServiceDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class IdentityServiceEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<IdentityServiceDbContext>(options =>
        {
            options.AddDefaultRepositories(includeAllEntities: true);
        });


        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure<IdentityServiceDbContext>(c =>
            {
                c.UseSqlServer();
            });
        });

    }
}

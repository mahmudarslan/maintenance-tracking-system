using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Arslan.Vms.VehicleService.EntityFrameworkCore;

[DependsOn(
    typeof(VehicleServiceDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class VehicleServiceEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
		context.Services.AddAbpDbContext<VehicleServiceDbContext>(options =>
		{
			options.AddDefaultRepositories(includeAllEntities: false);
		});


		Configure<AbpDbContextOptions>(options =>
		{
			options.Configure<VehicleServiceDbContext>(c =>
			{
				c.UseSqlServer();
			});
		});
	}
}

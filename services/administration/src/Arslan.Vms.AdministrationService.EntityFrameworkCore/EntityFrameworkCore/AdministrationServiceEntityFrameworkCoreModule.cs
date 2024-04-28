using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DistributedEvents;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Arslan.Vms.AdministrationService.EntityFrameworkCore;

[DependsOn(
    typeof(AdministrationServiceDomainModule),
	typeof(AbpPermissionManagementEntityFrameworkCoreModule),
	typeof(AbpSettingManagementEntityFrameworkCoreModule),
	typeof(AbpEntityFrameworkCoreSqlServerModule),
	//typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
	typeof(AbpAuditLoggingEntityFrameworkCoreModule),
	typeof(AbpTenantManagementEntityFrameworkCoreModule),
	typeof(AbpFeatureManagementEntityFrameworkCoreModule)
)]
public class AdministrationServiceEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<AdministrationServiceDbContext>(options =>
        {
			options.ReplaceDbContext<IPermissionManagementDbContext>();
			options.ReplaceDbContext<ISettingManagementDbContext>();
			options.ReplaceDbContext<IAuditLoggingDbContext>();
			//options.ReplaceDbContext<IBlobStoringDbContext>();
			options.ReplaceDbContext<IFeatureManagementDbContext>();
			options.ReplaceDbContext<ITenantManagementDbContext>();
			//options.AddRepository<Tenant, ICustomTenantRepository>();
			options.AddDefaultRepositories(includeAllEntities: false);
		});


        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure<AdministrationServiceDbContext>(c =>
            {
                c.UseSqlServer();
            });
        });

		//Configure<AbpDistributedEventBusOptions>(options =>
		//{
		//	options.Outboxes.Configure(config =>
		//	{
		//		config.UseDbContext<AdministrationServiceDbContext>();
		//	});
		//});
	}
}

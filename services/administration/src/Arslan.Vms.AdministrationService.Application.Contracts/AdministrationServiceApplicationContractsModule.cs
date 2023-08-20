using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace Arslan.Vms.AdministrationService;
[DependsOn(
    typeof(AdministrationServiceDomainSharedModule),
	typeof(AbpFeatureManagementApplicationContractsModule),
	typeof(AbpPermissionManagementApplicationContractsModule),
	typeof(AbpSettingManagementApplicationContractsModule),
	typeof(AbpTenantManagementApplicationContractsModule)

	)]
public class AdministrationServiceApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
    }
}
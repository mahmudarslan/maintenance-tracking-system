using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace Arslan.Vms.AdministrationService;
[DependsOn(
    typeof(AdministrationServiceApplicationContractsModule),
	typeof(AbpPermissionManagementHttpApiModule),
	typeof(AbpTenantManagementHttpApiModule),
	typeof(AbpFeatureManagementHttpApiModule),
	typeof(AbpSettingManagementHttpApiModule))]
public class AdministrationServiceHttpApiModule : AbpModule
{
}

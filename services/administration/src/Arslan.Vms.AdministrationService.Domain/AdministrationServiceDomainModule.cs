using Volo.Abp.AuditLogging;
using Volo.Abp.Domain;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace Arslan.Vms.AdministrationService;
[DependsOn(
typeof(AbpDddDomainModule),
    typeof(AdministrationServiceDomainSharedModule),
	typeof(AbpAuditLoggingDomainModule),
	typeof(AbpFeatureManagementDomainModule),
	typeof(AbpPermissionManagementDomainModule),
	typeof(AbpSettingManagementDomainModule),
	typeof(AbpTenantManagementDomainModule)
)]
public class AdministrationServiceDomainModule : AbpModule
{

}

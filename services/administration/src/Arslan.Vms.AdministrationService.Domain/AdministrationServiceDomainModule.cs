using Volo.Abp.AuditLogging;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Arslan.Vms.AdministrationService;
[DependsOn(
typeof(AbpDddDomainModule),
    typeof(AdministrationServiceDomainSharedModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpAuditLoggingDomainModule)
)]
public class AdministrationServiceDomainModule : AbpModule
{

}

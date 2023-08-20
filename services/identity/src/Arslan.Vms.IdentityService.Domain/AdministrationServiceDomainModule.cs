using Volo.Abp.AuditLogging;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Arslan.Vms.IdentityService;
[DependsOn(
typeof(AbpDddDomainModule),
    typeof(IdentityServiceDomainSharedModule),
        typeof(AbpAuditLoggingDomainModule)
)]
public class IdentityServiceDomainModule : AbpModule
{

}

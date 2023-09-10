using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Arslan.Vms.PlannerService;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(PlannerServiceDomainSharedModule)
)]
public class PlannerServiceDomainModule : AbpModule
{

}

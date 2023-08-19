using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Arslan.Vms.PlannerService;

[DependsOn(
    typeof(PlannerServiceDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class PlannerServiceApplicationContractsModule : AbpModule
{

}

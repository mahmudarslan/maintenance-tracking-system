using Volo.Abp.Modularity;

namespace Arslan.Vms.PlannerService;

[DependsOn(
    typeof(PlannerServiceApplicationModule),
    typeof(PlannerServiceDomainTestModule)
    )]
public class PlannerServiceApplicationTestModule : AbpModule
{

}

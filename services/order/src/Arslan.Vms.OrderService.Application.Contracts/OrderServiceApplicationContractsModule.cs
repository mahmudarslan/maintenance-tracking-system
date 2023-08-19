using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Arslan.Vms.OrderService;

[DependsOn(
    typeof(OrderServiceDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class OrderServiceApplicationContractsModule : AbpModule
{

}

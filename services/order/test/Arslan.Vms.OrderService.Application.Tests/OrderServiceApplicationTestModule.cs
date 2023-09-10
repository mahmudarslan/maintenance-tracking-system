using Volo.Abp.Modularity;

namespace Arslan.Vms.OrderService;

[DependsOn(
    typeof(OrderServiceApplicationModule),
    typeof(OrderServiceDomainTestModule)
    )]
public class OrderServiceApplicationTestModule : AbpModule
{

}

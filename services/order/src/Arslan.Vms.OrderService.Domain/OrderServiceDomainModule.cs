using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Arslan.Vms.OrderService;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(OrderServiceDomainSharedModule)
)]
public class OrderServiceDomainModule : AbpModule
{

}

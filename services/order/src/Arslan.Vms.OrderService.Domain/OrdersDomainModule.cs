//using Arslan.Vms.Base;
//using Arslan.Vms.Inventory;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Arslan.Vms.OrderService
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(OrderServiceDomainSharedModule)
    //typeof(BaseDomainModule),
    //typeof(InventoryDomainModule)
    )]
    public class OrdersDomainModule : AbpModule
    {

    }
}
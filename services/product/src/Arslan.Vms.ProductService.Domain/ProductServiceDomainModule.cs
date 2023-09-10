using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Arslan.Vms.ProductService;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ProductServiceDomainSharedModule)
)]
public class ProductServiceDomainModule : AbpModule
{

}

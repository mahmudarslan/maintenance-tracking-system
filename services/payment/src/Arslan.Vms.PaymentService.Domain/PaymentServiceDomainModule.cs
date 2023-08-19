using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Arslan.Vms.PaymentService;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(PaymentServiceDomainSharedModule)
)]
public class PaymentServiceDomainModule : AbpModule
{

}

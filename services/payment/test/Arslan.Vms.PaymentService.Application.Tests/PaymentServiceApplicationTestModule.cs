using Volo.Abp.Modularity;

namespace Arslan.Vms.PaymentService;

[DependsOn(
    typeof(PaymentServiceApplicationModule),
    typeof(PaymentServiceDomainTestModule)
    )]
public class PaymentServiceApplicationTestModule : AbpModule
{

}

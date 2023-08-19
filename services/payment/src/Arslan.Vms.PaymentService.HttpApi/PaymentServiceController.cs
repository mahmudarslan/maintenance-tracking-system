using Arslan.Vms.PaymentService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Arslan.Vms.PaymentService;

public abstract class PaymentServiceController : AbpControllerBase
{
    protected PaymentServiceController()
    {
        LocalizationResource = typeof(PaymentServiceResource);
    }
}

using Arslan.Vms.PaymentService.Localization;
using Volo.Abp.Application.Services;

namespace Arslan.Vms.PaymentService;

public abstract class PaymentServiceAppService : ApplicationService
{
    protected PaymentServiceAppService()
    {
        LocalizationResource = typeof(PaymentServiceResource);
        ObjectMapperContext = typeof(PaymentServiceApplicationModule);
    }
}

using Arslan.Vms.OrderService.Localization;
using Volo.Abp.Application.Services;

namespace Arslan.Vms.OrderService;

public abstract class OrderServiceAppService : ApplicationService
{
    protected OrderServiceAppService()
    {
        LocalizationResource = typeof(OrderServiceResource);
        ObjectMapperContext = typeof(OrderServiceApplicationModule);
    }
}

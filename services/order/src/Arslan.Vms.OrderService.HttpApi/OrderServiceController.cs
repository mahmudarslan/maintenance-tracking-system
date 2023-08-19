using Arslan.Vms.OrderService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Arslan.Vms.OrderService;

public abstract class OrderServiceController : AbpControllerBase
{
    protected OrderServiceController()
    {
        LocalizationResource = typeof(OrderServiceResource);
    }
}

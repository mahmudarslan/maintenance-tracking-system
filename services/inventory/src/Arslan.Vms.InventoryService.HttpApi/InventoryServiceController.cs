using Arslan.Vms.InventoryService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Arslan.Vms.InventoryService;

public abstract class InventoryServiceController : AbpControllerBase
{
    protected InventoryServiceController()
    {
        LocalizationResource = typeof(InventoryServiceResource);
    }
}

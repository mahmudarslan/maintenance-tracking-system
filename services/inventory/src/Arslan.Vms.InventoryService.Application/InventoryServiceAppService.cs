using Arslan.Vms.InventoryService.Localization;
using Volo.Abp.Application.Services;

namespace Arslan.Vms.InventoryService;

public abstract class InventoryServiceAppService : ApplicationService
{
    protected InventoryServiceAppService()
    {
        LocalizationResource = typeof(InventoryServiceResource);
        ObjectMapperContext = typeof(InventoryServiceApplicationModule);
    }
}

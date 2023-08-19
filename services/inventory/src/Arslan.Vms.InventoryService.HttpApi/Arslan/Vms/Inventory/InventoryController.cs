using Arslan.Vms.InventoryService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Arslan.Vms.Inventory
{
    public abstract class InventoryController : AbpController
    {
        protected InventoryController()
        {
            LocalizationResource = typeof(InventoryServiceResource);
        }
    }
}
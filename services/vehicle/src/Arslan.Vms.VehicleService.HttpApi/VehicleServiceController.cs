using Arslan.Vms.VehicleService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Arslan.Vms.VehicleService;

public abstract class VehicleServiceController : AbpControllerBase
{
    protected VehicleServiceController()
    {
        LocalizationResource = typeof(VehicleServiceResource);
    }
}

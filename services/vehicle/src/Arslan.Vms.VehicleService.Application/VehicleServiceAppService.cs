using Arslan.Vms.VehicleService.Localization;
using Volo.Abp.Application.Services;

namespace Arslan.Vms.VehicleService;

public abstract class VehicleServiceAppService : ApplicationService
{
    protected VehicleServiceAppService()
    {
        LocalizationResource = typeof(VehicleServiceResource);
        ObjectMapperContext = typeof(VehicleServiceApplicationModule);
    }
}

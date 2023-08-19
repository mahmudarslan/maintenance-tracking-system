using Arslan.Vms.AdministrationService.Localization;
using Volo.Abp.Application.Services;

namespace Arslan.Vms.AdministrationService;

public abstract class AdministrationServiceAppService : ApplicationService
{
    protected AdministrationServiceAppService()
    {
        LocalizationResource = typeof(AdministrationServiceResource);
        ObjectMapperContext = typeof(AdministrationServiceApplicationModule);
    }
}

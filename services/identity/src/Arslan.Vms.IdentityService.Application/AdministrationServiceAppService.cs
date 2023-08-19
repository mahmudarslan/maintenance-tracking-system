using Arslan.Vms.IdentityService.Localization;
using Volo.Abp.Application.Services;

namespace Arslan.Vms.IdentityService;

public abstract class AdministrationServiceAppService : ApplicationService
{
    protected AdministrationServiceAppService()
    {
        LocalizationResource = typeof(AdministrationServiceResource);
        ObjectMapperContext = typeof(AdministrationServiceApplicationModule);
    }
}

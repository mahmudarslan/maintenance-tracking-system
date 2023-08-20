using Arslan.Vms.IdentityService.Localization;
using Volo.Abp.Application.Services;

namespace Arslan.Vms.IdentityService;

public abstract class IdentityServiceAppService : ApplicationService
{
    protected IdentityServiceAppService()
    {
        LocalizationResource = typeof(IdentityServiceResource);
        ObjectMapperContext = typeof(IdentityServiceApplicationModule);
    }
}

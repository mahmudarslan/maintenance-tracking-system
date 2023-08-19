using Arslan.Vms.IdentityService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Arslan.Vms.IdentityService;

public abstract class AdministrationServiceController : AbpControllerBase
{
    protected AdministrationServiceController()
    {
        LocalizationResource = typeof(AdministrationServiceResource);
    }
}

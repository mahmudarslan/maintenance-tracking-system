using Arslan.Vms.IdentityService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Arslan.Vms.IdentityService;

public abstract class IdentityServiceController : AbpControllerBase
{
    protected IdentityServiceController()
    {
        LocalizationResource = typeof(IdentityServiceResource);
    }
}

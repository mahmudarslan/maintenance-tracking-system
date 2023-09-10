using Arslan.Vms.PlannerService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Arslan.Vms.PlannerService;

public abstract class PlannerServiceController : AbpControllerBase
{
    protected PlannerServiceController()
    {
        LocalizationResource = typeof(PlannerServiceResource);
    }
}

using Arslan.Vms.PlannerService.Localization;
using Volo.Abp.Application.Services;

namespace Arslan.Vms.PlannerService;

public abstract class PlannerServiceAppService : ApplicationService
{
    protected PlannerServiceAppService()
    {
        LocalizationResource = typeof(PlannerServiceResource);
        ObjectMapperContext = typeof(PlannerServiceApplicationModule);
    }
}

using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Arslan.Vms.Shared.Hosting.AspNetCore;

[Dependency(ReplaceServices = true)]
public class ArslanVmsBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "ArslanVms";
}

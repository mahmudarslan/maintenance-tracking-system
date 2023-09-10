using Arslan.Vms.PlannerService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Arslan.Vms.PlannerService.Permissions;

public class PlannerServicePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(PlannerServicePermissions.GroupName, L("Permission:PlannerService"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PlannerServiceResource>(name);
    }
}

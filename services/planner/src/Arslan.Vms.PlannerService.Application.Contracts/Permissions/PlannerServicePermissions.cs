using Volo.Abp.Reflection;

namespace Arslan.Vms.PlannerService.Permissions;

public class PlannerServicePermissions
{
    public const string GroupName = "PlannerService";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(PlannerServicePermissions));
    }
}

using Volo.Abp.Reflection;

namespace Arslan.Vms.OrderService.Permissions;

public class OrderServicePermissions
{
    public const string GroupName = "OrderService";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(OrderServicePermissions));
    }

    public static class SalesOrder
    {
        public const string Default = GroupName + ".SalesOrder";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Undo = Default + ".Undo";
        public const string List = Default + ".List";
    }

    public static class PurchaseOrder
    {
        public const string Default = GroupName + ".PurchaseOrder";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Undo = Default + ".Undo";
        public const string List = Default + ".List";
    }
}

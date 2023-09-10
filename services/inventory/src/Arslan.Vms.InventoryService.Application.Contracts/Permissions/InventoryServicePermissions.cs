using Volo.Abp.Reflection;

namespace Arslan.Vms.InventoryService.Permissions;

public class InventoryServicePermissions
{
    public const string GroupName = "InventoryService";

    public static class AdjustStock
    {
        public const string Default = GroupName + ".AdjustStock";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Undo = Default + ".Undo";
        public const string List = Default + ".List";
    }

    public static class CurrentStock
    {
        public const string Default = GroupName + ".CurrentStock";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Undo = Default + ".Undo";
        public const string List = Default + ".List";
    }

    //public static class ReorderStock
    //{
    //    public const string Default = GroupName + ".ReorderStock";
    //    public const string Create = Default + ".Create";
    //    public const string Update = Default + ".Update";
    //    public const string Delete = Default + ".Delete";
    //    public const string Undo = Default + ".Undo";
    //    public const string List = Default + ".List";
    //}

    //public static class Workorder
    //{
    //    public const string Default = GroupName + ".Workorder";
    //    public const string Create = Default + ".Create";
    //    public const string Update = Default + ".Update";
    //    public const string Delete = Default + ".Delete";
    //    public const string Undo = Default + ".Undo";
    //    public const string List = Default + ".List";
    //}

    //public static class Product
    //{
    //    public const string Default = GroupName + ".Product";
    //    public const string Create = Default + ".Create";
    //    public const string Update = Default + ".Update";
    //    public const string Delete = Default + ".Delete";
    //    public const string Undo = Default + ".Undo";
    //    public const string List = Default + ".List";
    //}

    //public static class Category
    //{
    //    public const string Default = GroupName + ".Category";
    //    public const string Create = Default + ".Create";
    //    public const string Update = Default + ".Update";
    //    public const string Delete = Default + ".Delete";
    //    public const string Undo = Default + ".Undo";
    //    public const string List = Default + ".List";
    //}

    //public static class Location
    //{
    //    public const string Default = GroupName + ".Location";
    //    public const string Create = Default + ".Create";
    //    public const string Update = Default + ".Update";
    //    public const string Delete = Default + ".Delete";
    //    public const string Undo = Default + ".Undo";
    //    public const string List = Default + ".List";
    //}

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(InventoryServicePermissions));
    }
}

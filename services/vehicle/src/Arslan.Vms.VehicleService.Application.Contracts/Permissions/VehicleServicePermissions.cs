using Volo.Abp.Reflection;

namespace Arslan.Vms.VehicleService.Permissions;

public class VehicleServicePermissions
{
    public const string GroupName = "VehicleService";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(VehicleServicePermissions));
    }

    public static class Vehicle
    {
        public const string Default = GroupName + ".Vehicle";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Undo = Default + ".Undo";
        public const string List = Default + ".List";
    }

    public static class VehicleType
    {
        public const string Default = GroupName + ".VehicleType";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Undo = Default + ".Undo";
        public const string List = Default + ".List";
    }

    public static class Product
    {
        public const string Default = GroupName + ".Product";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Undo = Default + ".Undo";
        public const string List = Default + ".List";
    }

    public static class Category
    {
        public const string Default = GroupName + ".Category";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Undo = Default + ".Undo";
        public const string List = Default + ".List";
    }

    public static class Location
    {
        public const string Default = GroupName + ".Location";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Undo = Default + ".Undo";
        public const string List = Default + ".List";
    }

    public static class DocNumbers
    {
        public const string Default = GroupName + ".DocNumber";
        //public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        //public const string Delete = Default + ".Delete";
        public const string List = Default + ".List";
    }

    public static class CurrencyConversion
    {
        public const string Default = GroupName + ".CurrencyConversion";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string List = Default + ".List";
    }

    public static class PricingScheme
    {
        public const string Default = GroupName + ".PricingScheme";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string List = Default + ".List";
    }

    public static class TaxingScheme
    {
        public const string Default = GroupName + ".TaxingScheme";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string List = Default + ".List";
    }

    public static class UploadFiles
    {
        public const string Default = GroupName + ".UploadFiles";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string List = Default + ".List";
        public const string Download = Default + ".Download";
        public const string Upload = Default + ".Upload";
    }
}

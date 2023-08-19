using Volo.Abp.Reflection;

namespace Arslan.Vms.IdentityService.Permissions;

public class IdentityServicePermissions
{
    public const string GroupName = "IdentityService";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(IdentityServicePermissions));
    }

    public static class Vendor
    {
        public const string Default = GroupName + ".Vendor";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Undo = Default + ".Undo";
        public const string List = Default + ".List";
    }

    public static class Customer
    {
        public const string Default = GroupName + ".Customer";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Undo = Default + ".Undo";
        public const string List = Default + ".List";
    }

    public static class Employee
    {
        public const string Default = GroupName + ".Employee";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Undo = Default + ".Undo";
        public const string List = Default + ".List";
    }

    public static class Company
    {
        public const string Default = GroupName + ".Company";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string List = Default + ".List";
    }

    public static class Address
    {
        public const string Default = GroupName + ".Address";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string List = Default + ".List";
    }

    public static class AddressType
    {
        public const string Default = GroupName + ".AddressType";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Undo = Default + ".Undo";
        public const string List = Default + ".List";
    }
}

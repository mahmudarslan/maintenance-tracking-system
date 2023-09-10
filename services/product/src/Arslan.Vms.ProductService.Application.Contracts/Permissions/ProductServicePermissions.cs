using Volo.Abp.Reflection;

namespace Arslan.Vms.ProductService.Permissions;

public class ProductServicePermissions
{
    public const string GroupName = "ProductService";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ProductServicePermissions));
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

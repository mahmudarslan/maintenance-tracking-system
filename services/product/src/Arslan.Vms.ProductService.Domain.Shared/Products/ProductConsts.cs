using System;
using System.Collections.Generic;
using System.Text;

namespace Arslan.Vms.ProductService.Products
{
    public static class RoleConsts
    {
        public static string Admin => "ADMIN";
        public static string Technician => "TECHNICIAN";
        public static string HeadTechnician => "HEAD TECHNICIAN";
        public static string Customer => "CUSTOMER";
        public static string Vendor => "VENDOR";
        public static string Owner => "OWNER";
    }

    public class VendorLineConsts
    {
        public static int VendorItemCodeMaxLength { get; set; } = 100;
    }

    public class VehicleTypeConsts
    {
        public static int NameMaxLength { get; set; } = 200;
    }

    public class VehicleConsts
    {
        public static int PlateMaxLength { get; set; } = 100;
        public static int ChassisMaxLength { get; set; } = 100;
        public static int ColorMaxLength { get; set; } = 50;
        public static int MotorMaxLength { get; set; } = 100;
    }

    public class UserConsts
    {
        public static int VendorPermitNumberMaxLength { get; set; } = 100;
        public static int DefaultCarrierMaxLength { get; set; } = 100;
        public static int DefaultPaymentMethodMaxLength { get; set; } = 100;
    }

    public class TaxingSchemeConsts
    {
        public static int NameMaxLength { get; set; } = 100;
        public static int Tax1NameMaxLength { get; set; } = 100;
        public static int Tax2NameMaxLength { get; set; } = 100;
    }

    public class TaxCodeConsts
    {
        public static int NameMaxLength { get; set; } = 100;
    }

    public class ProductConsts
    {
        public static int NameMaxLength { get; set; } = 200;
        public static int BarcodeMaxLength { get; set; } = 500;
        public static int DefaultSublocationMaxLength { get; set; } = 100;
        public static int UomMaxLength { get; set; } = 20;
    }

    public class PricingSchemeConsts
    {
        public static int NameMaxLength { get; set; } = 100;
    }

    public class LocationConsts
    {
        public static int NameMaxLength { get; set; } = 100;
    }

    public class DocNoFormatConsts
    {
        public static int PrefixMaxLength { get; set; } = 30;
        public static int SuffixMaxLength { get; set; } = 30;
    }

    public class CurrencyConsts
    {
        public static int CodeMaxLength { get; set; } = 5;
        public static int DescriptionMaxLength { get; set; } = 100;
        public static int SymbolMaxLength { get; set; } = 5;
        public static int DecimalSeparatorMaxLength { get; set; } = 5;
        public static int ThousandsSeparatorMaxLength { get; set; } = 5;
    }

    public class AddressTypeConsts
    {
        public static int NameMaxLength { get; set; } = 200;
    }



}

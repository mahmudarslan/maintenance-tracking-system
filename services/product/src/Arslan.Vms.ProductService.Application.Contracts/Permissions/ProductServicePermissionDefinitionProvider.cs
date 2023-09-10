using Arslan.Vms.ProductService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.Permissions;

public class ProductServicePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var baseGroup = context.AddGroup(ProductServicePermissions.GroupName, L("Permission:Base"));

        var vendorManagement = baseGroup.AddPermission(ProductServicePermissions.Vendor.Default, L("Permission:VendorManagement"), MultiTenancySides.Both, isEnabled: true);
        vendorManagement.AddChild(ProductServicePermissions.Vendor.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        vendorManagement.AddChild(ProductServicePermissions.Vendor.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        vendorManagement.AddChild(ProductServicePermissions.Vendor.Delete, L("Permission:Delete"), MultiTenancySides.Both, isEnabled: true);
        vendorManagement.AddChild(ProductServicePermissions.Vendor.Undo, L("Permission:Undo"), MultiTenancySides.Both, isEnabled: true);
        vendorManagement.AddChild(ProductServicePermissions.Vendor.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var customerManagement = baseGroup.AddPermission(ProductServicePermissions.Customer.Default, L("Permission:CustomerManagement"), MultiTenancySides.Both, isEnabled: true);
        customerManagement.AddChild(ProductServicePermissions.Customer.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        customerManagement.AddChild(ProductServicePermissions.Customer.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        customerManagement.AddChild(ProductServicePermissions.Customer.Delete, L("Permission:Delete"), MultiTenancySides.Both, isEnabled: true);
        customerManagement.AddChild(ProductServicePermissions.Customer.Undo, L("Permission:Undo"), MultiTenancySides.Both, isEnabled: true);
        customerManagement.AddChild(ProductServicePermissions.Customer.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var employeeManagement = baseGroup.AddPermission(ProductServicePermissions.Employee.Default, L("Permission:EmployeeManagement"), MultiTenancySides.Both, isEnabled: true);
        employeeManagement.AddChild(ProductServicePermissions.Employee.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        employeeManagement.AddChild(ProductServicePermissions.Employee.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        employeeManagement.AddChild(ProductServicePermissions.Employee.Delete, L("Permission:Delete"), MultiTenancySides.Both, isEnabled: true);
        employeeManagement.AddChild(ProductServicePermissions.Employee.Undo, L("Permission:Undo"), MultiTenancySides.Both, isEnabled: true);
        employeeManagement.AddChild(ProductServicePermissions.Employee.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var vehilceManagement = baseGroup.AddPermission(ProductServicePermissions.Vehicle.Default, L("Permission:VehicleManagement"), MultiTenancySides.Both, isEnabled: true);
        vehilceManagement.AddChild(ProductServicePermissions.Vehicle.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        vehilceManagement.AddChild(ProductServicePermissions.Vehicle.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        vehilceManagement.AddChild(ProductServicePermissions.Vehicle.Delete, L("Permission:Delete"), MultiTenancySides.Both, isEnabled: true);
        vehilceManagement.AddChild(ProductServicePermissions.Vehicle.Undo, L("Permission:Undo"), MultiTenancySides.Both, isEnabled: true);
        vehilceManagement.AddChild(ProductServicePermissions.Vehicle.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var vehilceTypeManagement = baseGroup.AddPermission(ProductServicePermissions.VehicleType.Default, L("Permission:VehicleTypeManagement"), MultiTenancySides.Both, isEnabled: true);
        vehilceTypeManagement.AddChild(ProductServicePermissions.VehicleType.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        vehilceTypeManagement.AddChild(ProductServicePermissions.VehicleType.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        vehilceTypeManagement.AddChild(ProductServicePermissions.VehicleType.Delete, L("Permission:Delete"), MultiTenancySides.Both, isEnabled: true);
        vehilceTypeManagement.AddChild(ProductServicePermissions.VehicleType.Undo, L("Permission:Undo"), MultiTenancySides.Both, isEnabled: true);
        vehilceTypeManagement.AddChild(ProductServicePermissions.VehicleType.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var companyManagement = baseGroup.AddPermission(ProductServicePermissions.Company.Default, L("Permission:CompanyManagement"), MultiTenancySides.Both, isEnabled: true);
        companyManagement.AddChild(ProductServicePermissions.Company.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        companyManagement.AddChild(ProductServicePermissions.Company.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        companyManagement.AddChild(ProductServicePermissions.Company.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var addressManagement = baseGroup.AddPermission(ProductServicePermissions.Address.Default, L("Permission:AddressManagement"), MultiTenancySides.Both, isEnabled: true);
        addressManagement.AddChild(ProductServicePermissions.Address.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        addressManagement.AddChild(ProductServicePermissions.Address.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        addressManagement.AddChild(ProductServicePermissions.Address.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var addressTypeManagement = baseGroup.AddPermission(ProductServicePermissions.AddressType.Default, L("Permission:AddressTypeManagement"), MultiTenancySides.Both, isEnabled: true);
        addressTypeManagement.AddChild(ProductServicePermissions.AddressType.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        addressTypeManagement.AddChild(ProductServicePermissions.AddressType.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        addressTypeManagement.AddChild(ProductServicePermissions.AddressType.Delete, L("Permission:Delete"), MultiTenancySides.Both, isEnabled: true);
        addressTypeManagement.AddChild(ProductServicePermissions.AddressType.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var productManagement = baseGroup.AddPermission(ProductServicePermissions.Product.Default, L("Permission:ProductManagement"));
        productManagement.AddChild(ProductServicePermissions.Product.Create, L("Permission:Create"));
        productManagement.AddChild(ProductServicePermissions.Product.Update, L("Permission:Edit"));
        productManagement.AddChild(ProductServicePermissions.Product.Delete, L("Permission:Delete"));
        productManagement.AddChild(ProductServicePermissions.Product.Undo, L("Permission:Undo"));
        productManagement.AddChild(ProductServicePermissions.Product.List, L("Permission:List"));

        var categoryManagement = baseGroup.AddPermission(ProductServicePermissions.Category.Default, L("Permission:CategoryManagement"));
        categoryManagement.AddChild(ProductServicePermissions.Category.Create, L("Permission:Create"));
        categoryManagement.AddChild(ProductServicePermissions.Category.Update, L("Permission:Edit"));
        categoryManagement.AddChild(ProductServicePermissions.Category.Delete, L("Permission:Delete"));
        categoryManagement.AddChild(ProductServicePermissions.Category.Undo, L("Permission:Undo"));
        categoryManagement.AddChild(ProductServicePermissions.Category.List, L("Permission:List"));

        var locationManagement = baseGroup.AddPermission(ProductServicePermissions.Location.Default, L("Permission:LocationManagement"));
        locationManagement.AddChild(ProductServicePermissions.Location.Create, L("Permission:Create"));
        locationManagement.AddChild(ProductServicePermissions.Location.Update, L("Permission:Edit"));
        locationManagement.AddChild(ProductServicePermissions.Location.Delete, L("Permission:Delete"));
        locationManagement.AddChild(ProductServicePermissions.Location.Undo, L("Permission:Undo"));
        locationManagement.AddChild(ProductServicePermissions.Location.List, L("Permission:List"));

        var docNumberManagement = baseGroup.AddPermission(ProductServicePermissions.DocNumbers.Default, L("Permission:DocNumberManagement"));
        //docNumberManagement.AddChild(CorePermissions.DocNumbers.Create, L("Permission:Create"));
        docNumberManagement.AddChild(ProductServicePermissions.DocNumbers.Update, L("Permission:Edit"));
        //docNumberManagement.AddChild(CorePermissions.DocNumbers.Delete, L("Permission:Delete"));
        docNumberManagement.AddChild(ProductServicePermissions.DocNumbers.List, L("Permission:List"));

        var currencyConversionManagement = baseGroup.AddPermission(ProductServicePermissions.CurrencyConversion.Default, L("Permission:CurrencyConversionManagement"));
        currencyConversionManagement.AddChild(ProductServicePermissions.CurrencyConversion.Create, L("Permission:Create"));
        currencyConversionManagement.AddChild(ProductServicePermissions.CurrencyConversion.Update, L("Permission:Edit"));
        currencyConversionManagement.AddChild(ProductServicePermissions.CurrencyConversion.Delete, L("Permission:Delete"));
        currencyConversionManagement.AddChild(ProductServicePermissions.CurrencyConversion.List, L("Permission:List"));

        var pricingShemeManagement = baseGroup.AddPermission(ProductServicePermissions.PricingScheme.Default, L("Permission:PricingSchemeManagement"));
        pricingShemeManagement.AddChild(ProductServicePermissions.PricingScheme.Create, L("Permission:Create"));
        pricingShemeManagement.AddChild(ProductServicePermissions.PricingScheme.Update, L("Permission:Edit"));
        pricingShemeManagement.AddChild(ProductServicePermissions.PricingScheme.Delete, L("Permission:Delete"));
        pricingShemeManagement.AddChild(ProductServicePermissions.PricingScheme.List, L("Permission:List"));

        var taxingSchemeManagement = baseGroup.AddPermission(ProductServicePermissions.TaxingScheme.Default, L("Permission:TaxingSchemeManagement"));
        taxingSchemeManagement.AddChild(ProductServicePermissions.TaxingScheme.Create, L("Permission:Create"));
        taxingSchemeManagement.AddChild(ProductServicePermissions.TaxingScheme.Update, L("Permission:Edit"));
        taxingSchemeManagement.AddChild(ProductServicePermissions.TaxingScheme.Delete, L("Permission:Delete"));
        taxingSchemeManagement.AddChild(ProductServicePermissions.TaxingScheme.List, L("Permission:List"));

        var uploadFilesManagement = baseGroup.AddPermission(ProductServicePermissions.UploadFiles.Default, L("Permission:UploadFilesManagement"));
        uploadFilesManagement.AddChild(ProductServicePermissions.UploadFiles.Create, L("Permission:Create"));
        uploadFilesManagement.AddChild(ProductServicePermissions.UploadFiles.Update, L("Permission:Edit"));
        uploadFilesManagement.AddChild(ProductServicePermissions.UploadFiles.Delete, L("Permission:Delete"));
        uploadFilesManagement.AddChild(ProductServicePermissions.UploadFiles.List, L("Permission:List"));
        uploadFilesManagement.AddChild(ProductServicePermissions.UploadFiles.Upload, L("Permission:Upload"));
        uploadFilesManagement.AddChild(ProductServicePermissions.UploadFiles.Download, L("Permission:Download"));

    }


    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ProductServiceResource>(name);
    }
}

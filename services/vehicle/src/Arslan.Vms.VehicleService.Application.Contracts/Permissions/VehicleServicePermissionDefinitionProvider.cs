using Arslan.Vms.VehicleService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.VehicleService.Permissions;

public class VehicleServicePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var baseGroup = context.AddGroup(VehicleServicePermissions.GroupName, L("Permission:Base"), MultiTenancySides.Both);

        var vehilceManagement = baseGroup.AddPermission(VehicleServicePermissions.Vehicle.Default, L("Permission:VehicleManagement"), MultiTenancySides.Both, isEnabled: true);
        vehilceManagement.AddChild(VehicleServicePermissions.Vehicle.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        vehilceManagement.AddChild(VehicleServicePermissions.Vehicle.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        vehilceManagement.AddChild(VehicleServicePermissions.Vehicle.Delete, L("Permission:Delete"), MultiTenancySides.Both, isEnabled: true);
        vehilceManagement.AddChild(VehicleServicePermissions.Vehicle.Undo, L("Permission:Undo"), MultiTenancySides.Both, isEnabled: true);
        vehilceManagement.AddChild(VehicleServicePermissions.Vehicle.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var vehilceTypeManagement = baseGroup.AddPermission(VehicleServicePermissions.VehicleType.Default, L("Permission:VehicleTypeManagement"), MultiTenancySides.Both, isEnabled: true);
        vehilceTypeManagement.AddChild(VehicleServicePermissions.VehicleType.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        vehilceTypeManagement.AddChild(VehicleServicePermissions.VehicleType.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        vehilceTypeManagement.AddChild(VehicleServicePermissions.VehicleType.Delete, L("Permission:Delete"), MultiTenancySides.Both, isEnabled: true);
        vehilceTypeManagement.AddChild(VehicleServicePermissions.VehicleType.Undo, L("Permission:Undo"), MultiTenancySides.Both, isEnabled: true);
        vehilceTypeManagement.AddChild(VehicleServicePermissions.VehicleType.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var productManagement = baseGroup.AddPermission(VehicleServicePermissions.Product.Default, L("Permission:ProductManagement"));
        productManagement.AddChild(VehicleServicePermissions.Product.Create, L("Permission:Create"));
        productManagement.AddChild(VehicleServicePermissions.Product.Update, L("Permission:Edit"));
        productManagement.AddChild(VehicleServicePermissions.Product.Delete, L("Permission:Delete"));
        productManagement.AddChild(VehicleServicePermissions.Product.Undo, L("Permission:Undo"));
        productManagement.AddChild(VehicleServicePermissions.Product.List, L("Permission:List"));

        var categoryManagement = baseGroup.AddPermission(VehicleServicePermissions.Category.Default, L("Permission:CategoryManagement"));
        categoryManagement.AddChild(VehicleServicePermissions.Category.Create, L("Permission:Create"));
        categoryManagement.AddChild(VehicleServicePermissions.Category.Update, L("Permission:Edit"));
        categoryManagement.AddChild(VehicleServicePermissions.Category.Delete, L("Permission:Delete"));
        categoryManagement.AddChild(VehicleServicePermissions.Category.Undo, L("Permission:Undo"));
        categoryManagement.AddChild(VehicleServicePermissions.Category.List, L("Permission:List"));

        var locationManagement = baseGroup.AddPermission(VehicleServicePermissions.Location.Default, L("Permission:LocationManagement"));
        locationManagement.AddChild(VehicleServicePermissions.Location.Create, L("Permission:Create"));
        locationManagement.AddChild(VehicleServicePermissions.Location.Update, L("Permission:Edit"));
        locationManagement.AddChild(VehicleServicePermissions.Location.Delete, L("Permission:Delete"));
        locationManagement.AddChild(VehicleServicePermissions.Location.Undo, L("Permission:Undo"));
        locationManagement.AddChild(VehicleServicePermissions.Location.List, L("Permission:List"));

        var docNumberManagement = baseGroup.AddPermission(VehicleServicePermissions.DocNumbers.Default, L("Permission:DocNumberManagement"));
        //docNumberManagement.AddChild(CorePermissions.DocNumbers.Create, L("Permission:Create"));
        docNumberManagement.AddChild(VehicleServicePermissions.DocNumbers.Update, L("Permission:Edit"));
        //docNumberManagement.AddChild(CorePermissions.DocNumbers.Delete, L("Permission:Delete"));
        docNumberManagement.AddChild(VehicleServicePermissions.DocNumbers.List, L("Permission:List"));

        var currencyConversionManagement = baseGroup.AddPermission(VehicleServicePermissions.CurrencyConversion.Default, L("Permission:CurrencyConversionManagement"));
        currencyConversionManagement.AddChild(VehicleServicePermissions.CurrencyConversion.Create, L("Permission:Create"));
        currencyConversionManagement.AddChild(VehicleServicePermissions.CurrencyConversion.Update, L("Permission:Edit"));
        currencyConversionManagement.AddChild(VehicleServicePermissions.CurrencyConversion.Delete, L("Permission:Delete"));
        currencyConversionManagement.AddChild(VehicleServicePermissions.CurrencyConversion.List, L("Permission:List"));

        var pricingShemeManagement = baseGroup.AddPermission(VehicleServicePermissions.PricingScheme.Default, L("Permission:PricingSchemeManagement"));
        pricingShemeManagement.AddChild(VehicleServicePermissions.PricingScheme.Create, L("Permission:Create"));
        pricingShemeManagement.AddChild(VehicleServicePermissions.PricingScheme.Update, L("Permission:Edit"));
        pricingShemeManagement.AddChild(VehicleServicePermissions.PricingScheme.Delete, L("Permission:Delete"));
        pricingShemeManagement.AddChild(VehicleServicePermissions.PricingScheme.List, L("Permission:List"));

        var taxingSchemeManagement = baseGroup.AddPermission(VehicleServicePermissions.TaxingScheme.Default, L("Permission:TaxingSchemeManagement"));
        taxingSchemeManagement.AddChild(VehicleServicePermissions.TaxingScheme.Create, L("Permission:Create"));
        taxingSchemeManagement.AddChild(VehicleServicePermissions.TaxingScheme.Update, L("Permission:Edit"));
        taxingSchemeManagement.AddChild(VehicleServicePermissions.TaxingScheme.Delete, L("Permission:Delete"));
        taxingSchemeManagement.AddChild(VehicleServicePermissions.TaxingScheme.List, L("Permission:List"));

        var uploadFilesManagement = baseGroup.AddPermission(VehicleServicePermissions.UploadFiles.Default, L("Permission:UploadFilesManagement"));
        uploadFilesManagement.AddChild(VehicleServicePermissions.UploadFiles.Create, L("Permission:Create"));
        uploadFilesManagement.AddChild(VehicleServicePermissions.UploadFiles.Update, L("Permission:Edit"));
        uploadFilesManagement.AddChild(VehicleServicePermissions.UploadFiles.Delete, L("Permission:Delete"));
        uploadFilesManagement.AddChild(VehicleServicePermissions.UploadFiles.List, L("Permission:List"));
        uploadFilesManagement.AddChild(VehicleServicePermissions.UploadFiles.Upload, L("Permission:Upload"));
        uploadFilesManagement.AddChild(VehicleServicePermissions.UploadFiles.Download, L("Permission:Download"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<VehicleServiceResource>(name);
    }
}

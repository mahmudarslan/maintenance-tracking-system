using Arslan.Vms.IdentityService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.IdentityService.Permissions;

public class AdministrationServicePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var baseGroup = context.AddGroup(AdministrationServicePermissions.GroupName, L("Permission:AdministrationService"));

        var vendorManagement = baseGroup.AddPermission(AdministrationServicePermissions.Vendor.Default, L("Permission:VendorManagement"), MultiTenancySides.Both, isEnabled: true);
        vendorManagement.AddChild(AdministrationServicePermissions.Vendor.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        vendorManagement.AddChild(AdministrationServicePermissions.Vendor.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        vendorManagement.AddChild(AdministrationServicePermissions.Vendor.Delete, L("Permission:Delete"), MultiTenancySides.Both, isEnabled: true);
        vendorManagement.AddChild(AdministrationServicePermissions.Vendor.Undo, L("Permission:Undo"), MultiTenancySides.Both, isEnabled: true);
        vendorManagement.AddChild(AdministrationServicePermissions.Vendor.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var customerManagement = baseGroup.AddPermission(AdministrationServicePermissions.Customer.Default, L("Permission:CustomerManagement"), MultiTenancySides.Both, isEnabled: true);
        customerManagement.AddChild(AdministrationServicePermissions.Customer.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        customerManagement.AddChild(AdministrationServicePermissions.Customer.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        customerManagement.AddChild(AdministrationServicePermissions.Customer.Delete, L("Permission:Delete"), MultiTenancySides.Both, isEnabled: true);
        customerManagement.AddChild(AdministrationServicePermissions.Customer.Undo, L("Permission:Undo"), MultiTenancySides.Both, isEnabled: true);
        customerManagement.AddChild(AdministrationServicePermissions.Customer.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var employeeManagement = baseGroup.AddPermission(AdministrationServicePermissions.Employee.Default, L("Permission:EmployeeManagement"), MultiTenancySides.Both, isEnabled: true);
        employeeManagement.AddChild(AdministrationServicePermissions.Employee.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        employeeManagement.AddChild(AdministrationServicePermissions.Employee.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        employeeManagement.AddChild(AdministrationServicePermissions.Employee.Delete, L("Permission:Delete"), MultiTenancySides.Both, isEnabled: true);
        employeeManagement.AddChild(AdministrationServicePermissions.Employee.Undo, L("Permission:Undo"), MultiTenancySides.Both, isEnabled: true);
        employeeManagement.AddChild(AdministrationServicePermissions.Employee.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var companyManagement = baseGroup.AddPermission(AdministrationServicePermissions.Company.Default, L("Permission:CompanyManagement"), MultiTenancySides.Both, isEnabled: true);
        companyManagement.AddChild(AdministrationServicePermissions.Company.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        companyManagement.AddChild(AdministrationServicePermissions.Company.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        companyManagement.AddChild(AdministrationServicePermissions.Company.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var addressManagement = baseGroup.AddPermission(AdministrationServicePermissions.Address.Default, L("Permission:AddressManagement"), MultiTenancySides.Both, isEnabled: true);
        addressManagement.AddChild(AdministrationServicePermissions.Address.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        addressManagement.AddChild(AdministrationServicePermissions.Address.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        addressManagement.AddChild(AdministrationServicePermissions.Address.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var addressTypeManagement = baseGroup.AddPermission(AdministrationServicePermissions.AddressType.Default, L("Permission:AddressTypeManagement"), MultiTenancySides.Both, isEnabled: true);
        addressTypeManagement.AddChild(AdministrationServicePermissions.AddressType.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        addressTypeManagement.AddChild(AdministrationServicePermissions.AddressType.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        addressTypeManagement.AddChild(AdministrationServicePermissions.AddressType.Delete, L("Permission:Delete"), MultiTenancySides.Both, isEnabled: true);
        addressTypeManagement.AddChild(AdministrationServicePermissions.AddressType.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AdministrationServiceResource>(name);
    }
}

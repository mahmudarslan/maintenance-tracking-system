using Arslan.Vms.IdentityService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.IdentityService.Permissions;

public class IdentityServicePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var baseGroup = context.AddGroup(IdentityServicePermissions.GroupName, L("Permission:IdentityService"));

        var vendorManagement = baseGroup.AddPermission(IdentityServicePermissions.Vendor.Default, L("Permission:VendorManagement"), MultiTenancySides.Both, isEnabled: true);
        vendorManagement.AddChild(IdentityServicePermissions.Vendor.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        vendorManagement.AddChild(IdentityServicePermissions.Vendor.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        vendorManagement.AddChild(IdentityServicePermissions.Vendor.Delete, L("Permission:Delete"), MultiTenancySides.Both, isEnabled: true);
        vendorManagement.AddChild(IdentityServicePermissions.Vendor.Undo, L("Permission:Undo"), MultiTenancySides.Both, isEnabled: true);
        vendorManagement.AddChild(IdentityServicePermissions.Vendor.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var customerManagement = baseGroup.AddPermission(IdentityServicePermissions.Customer.Default, L("Permission:CustomerManagement"), MultiTenancySides.Both, isEnabled: true);
        customerManagement.AddChild(IdentityServicePermissions.Customer.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        customerManagement.AddChild(IdentityServicePermissions.Customer.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        customerManagement.AddChild(IdentityServicePermissions.Customer.Delete, L("Permission:Delete"), MultiTenancySides.Both, isEnabled: true);
        customerManagement.AddChild(IdentityServicePermissions.Customer.Undo, L("Permission:Undo"), MultiTenancySides.Both, isEnabled: true);
        customerManagement.AddChild(IdentityServicePermissions.Customer.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var employeeManagement = baseGroup.AddPermission(IdentityServicePermissions.Employee.Default, L("Permission:EmployeeManagement"), MultiTenancySides.Both, isEnabled: true);
        employeeManagement.AddChild(IdentityServicePermissions.Employee.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        employeeManagement.AddChild(IdentityServicePermissions.Employee.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        employeeManagement.AddChild(IdentityServicePermissions.Employee.Delete, L("Permission:Delete"), MultiTenancySides.Both, isEnabled: true);
        employeeManagement.AddChild(IdentityServicePermissions.Employee.Undo, L("Permission:Undo"), MultiTenancySides.Both, isEnabled: true);
        employeeManagement.AddChild(IdentityServicePermissions.Employee.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var companyManagement = baseGroup.AddPermission(IdentityServicePermissions.Company.Default, L("Permission:CompanyManagement"), MultiTenancySides.Both, isEnabled: true);
        companyManagement.AddChild(IdentityServicePermissions.Company.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        companyManagement.AddChild(IdentityServicePermissions.Company.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        companyManagement.AddChild(IdentityServicePermissions.Company.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var addressManagement = baseGroup.AddPermission(IdentityServicePermissions.Address.Default, L("Permission:AddressManagement"), MultiTenancySides.Both, isEnabled: true);
        addressManagement.AddChild(IdentityServicePermissions.Address.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        addressManagement.AddChild(IdentityServicePermissions.Address.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        addressManagement.AddChild(IdentityServicePermissions.Address.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);

        var addressTypeManagement = baseGroup.AddPermission(IdentityServicePermissions.AddressType.Default, L("Permission:AddressTypeManagement"), MultiTenancySides.Both, isEnabled: true);
        addressTypeManagement.AddChild(IdentityServicePermissions.AddressType.Create, L("Permission:Create"), MultiTenancySides.Both, isEnabled: true);
        addressTypeManagement.AddChild(IdentityServicePermissions.AddressType.Update, L("Permission:Edit"), MultiTenancySides.Both, isEnabled: true);
        addressTypeManagement.AddChild(IdentityServicePermissions.AddressType.Delete, L("Permission:Delete"), MultiTenancySides.Both, isEnabled: true);
        addressTypeManagement.AddChild(IdentityServicePermissions.AddressType.List, L("Permission:List"), MultiTenancySides.Both, isEnabled: true);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityServiceResource>(name);
    }
}

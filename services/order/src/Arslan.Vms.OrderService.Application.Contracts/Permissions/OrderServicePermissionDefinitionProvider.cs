using Arslan.Vms.OrderService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Arslan.Vms.OrderService.Permissions;

public class OrderServicePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var orderGroup = context.AddGroup(OrderServicePermissions.GroupName, L("Permission:Orders"));

        var soManagement = orderGroup.AddPermission(OrderServicePermissions.SalesOrder.Default, L("Permission:SalesOrderManagement"));
        soManagement.AddChild(OrderServicePermissions.SalesOrder.Create, L("Permission:Create"));
        soManagement.AddChild(OrderServicePermissions.SalesOrder.Update, L("Permission:Edit"));
        soManagement.AddChild(OrderServicePermissions.SalesOrder.Delete, L("Permission:Delete"));
        soManagement.AddChild(OrderServicePermissions.SalesOrder.Undo, L("Permission:Undo"));
        soManagement.AddChild(OrderServicePermissions.SalesOrder.List, L("Permission:List"));

        var poManagement = orderGroup.AddPermission(OrderServicePermissions.PurchaseOrder.Default, L("Permission:PurchaseOrderManagement"));
        poManagement.AddChild(OrderServicePermissions.PurchaseOrder.Create, L("Permission:Create"));
        poManagement.AddChild(OrderServicePermissions.PurchaseOrder.Update, L("Permission:Edit"));
        poManagement.AddChild(OrderServicePermissions.PurchaseOrder.Delete, L("Permission:Delete"));
        poManagement.AddChild(OrderServicePermissions.PurchaseOrder.Undo, L("Permission:Undo"));
        poManagement.AddChild(OrderServicePermissions.PurchaseOrder.List, L("Permission:List"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OrderServiceResource>(name);
    }
}

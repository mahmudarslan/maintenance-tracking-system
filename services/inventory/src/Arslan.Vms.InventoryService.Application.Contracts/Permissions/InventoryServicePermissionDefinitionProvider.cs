using Arslan.Vms.InventoryService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Arslan.Vms.InventoryService.Permissions;

public class InventoryServicePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var inventoryGroup = context.AddGroup(InventoryServicePermissions.GroupName, L("Permission:InventoryManagement"));

        var adjustStockManagement = inventoryGroup.AddPermission(InventoryServicePermissions.AdjustStock.Default, L("Permission:AdjustStockManagement"));
        adjustStockManagement.AddChild(InventoryServicePermissions.AdjustStock.Create, L("Permission:Create"));
        adjustStockManagement.AddChild(InventoryServicePermissions.AdjustStock.Update, L("Permission:Edit"));
        adjustStockManagement.AddChild(InventoryServicePermissions.AdjustStock.Delete, L("Permission:Delete"));
        adjustStockManagement.AddChild(InventoryServicePermissions.AdjustStock.Undo, L("Permission:Undo"));
        adjustStockManagement.AddChild(InventoryServicePermissions.AdjustStock.List, L("Permission:List"));

        var currentStockManagement = inventoryGroup.AddPermission(InventoryServicePermissions.CurrentStock.Default, L("Permission:CurrentStockManagement"));
        currentStockManagement.AddChild(InventoryServicePermissions.CurrentStock.Create, L("Permission:Create"));
        currentStockManagement.AddChild(InventoryServicePermissions.CurrentStock.Update, L("Permission:Edit"));
        currentStockManagement.AddChild(InventoryServicePermissions.CurrentStock.Delete, L("Permission:Delete"));
        currentStockManagement.AddChild(InventoryServicePermissions.CurrentStock.Undo, L("Permission:Undo"));
        currentStockManagement.AddChild(InventoryServicePermissions.CurrentStock.List, L("Permission:List"));

        //var reorderStockManagement = inventoryGroup.AddPermission(InventoryServicePermissions.ReorderStock.Default, L("Permission:ReorderStockManagement"));
        //reorderStockManagement.AddChild(InventoryServicePermissions.ReorderStock.Create, L("Permission:Create"));
        //reorderStockManagement.AddChild(InventoryServicePermissions.ReorderStock.Update, L("Permission:Edit"));
        //reorderStockManagement.AddChild(InventoryServicePermissions.ReorderStock.Delete, L("Permission:Delete"));
        //reorderStockManagement.AddChild(InventoryServicePermissions.ReorderStock.Undo, L("Permission:Undo"));
        //reorderStockManagement.AddChild(InventoryServicePermissions.ReorderStock.List, L("Permission:List"));

        //var workorderManagement = inventoryGroup.AddPermission(InventoryServicePermissions.Workorder.Default, L("Permission:WorkorderManagement"));
        //workorderManagement.AddChild(InventoryServicePermissions.Workorder.Create, L("Permission:Create"));
        //workorderManagement.AddChild(InventoryServicePermissions.Workorder.Update, L("Permission:Edit"));
        //workorderManagement.AddChild(InventoryServicePermissions.Workorder.Delete, L("Permission:Delete"));
        //workorderManagement.AddChild(InventoryServicePermissions.Workorder.Undo, L("Permission:Undo"));
        //workorderManagement.AddChild(InventoryServicePermissions.Workorder.List, L("Permission:List"));

        //var productManagement = inventoryGroup.AddPermission(InventoryServicePermissions.Product.Default, L("Permission:ProductManagement"));
        //productManagement.AddChild(InventoryServicePermissions.Product.Create, L("Permission:Create"));
        //productManagement.AddChild(InventoryServicePermissions.Product.Update, L("Permission:Edit"));
        //productManagement.AddChild(InventoryServicePermissions.Product.Delete, L("Permission:Delete"));
        //productManagement.AddChild(InventoryServicePermissions.Product.Undo, L("Permission:Undo"));
        //productManagement.AddChild(InventoryServicePermissions.Product.List, L("Permission:List"));

        //var categoryManagement = inventoryGroup.AddPermission(InventoryServicePermissions.Category.Default, L("Permission:CategoryManagement"));
        //categoryManagement.AddChild(InventoryServicePermissions.Category.Create, L("Permission:Create"));
        //categoryManagement.AddChild(InventoryServicePermissions.Category.Update, L("Permission:Edit"));
        //categoryManagement.AddChild(InventoryServicePermissions.Category.Delete, L("Permission:Delete"));
        //categoryManagement.AddChild(InventoryServicePermissions.Category.Undo, L("Permission:Undo"));
        //categoryManagement.AddChild(InventoryServicePermissions.Category.List, L("Permission:List"));

        //var locationManagement = inventoryGroup.AddPermission(InventoryServicePermissions.Location.Default, L("Permission:LocationManagement"));
        //locationManagement.AddChild(InventoryServicePermissions.Location.Create, L("Permission:Create"));
        //locationManagement.AddChild(InventoryServicePermissions.Location.Update, L("Permission:Edit"));
        //locationManagement.AddChild(InventoryServicePermissions.Location.Delete, L("Permission:Delete"));
        //locationManagement.AddChild(InventoryServicePermissions.Location.Undo, L("Permission:Undo"));
        //locationManagement.AddChild(InventoryServicePermissions.Location.List, L("Permission:List"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<InventoryServiceResource>(name);
    }
}

export const enum eInventoryPolicyNames {
    Inventory = 'Inventory.Product || Inventory.AdjustStock || Inventory.CurrentStockList || Inventory.ReorderStock || Inventory.Workorder',

    Product = 'Base.Product || Base.Product.List',
    NewProduct = 'Base.Product.Create || Base.Product.Update',
    ProductList = 'Base.Product.List || Base.Product.Delete || Base.Product.Undo',

    AdjustStock = 'Inventory.AdjustStock || Inventory.AdjustStock.List',
    AdjustStockNew = 'Inventory.AdjustStock.Create || Inventory.AdjustStock.Update',
    AdjustStockList = 'Inventory.AdjustStock.List || Inventory.AdjustStock.Delete || Inventory.AdjustStock.Undo',

    CurrentStock = 'Inventory.CurrentStockList',

    ReorderStock = 'Inventory.ReorderStock || Inventory.ReorderStock.List',

    Workorder = 'Inventory.Workorder || Inventory.Workorder.List',

    InventoryManagement = 'Base.Category || Base.Location',
    Category = 'Base.Category.Create ||Base.Category.Update || Base.Category.List',
    Location = 'Base.Location.Create ||Base.Location.Update || Base.Location.List',
}
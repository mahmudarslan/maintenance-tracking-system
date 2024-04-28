export const enum eOrderPolicyNames {
  SalesOrder = 'Order.SalesOrder',
  SalesOrderNew = 'Order.SalesOrder.Create || Order.SalesOrder.Update',
  SalesOrderList = 'Order.SalesOrder.List || Order.SalesOrder.Delete || Order.SalesOrder.Undo',

  PurchaseOrder = 'Order.PurchaseOrder',
  PurchaseOrderNew = 'Order.PurchaseOrder.Create || Order.PurchaseOrder.Update',
  PurchaseOrderList = 'Order.PurchaseOrder.List || Order.PurchaseOrder.Delete  || Order.PurchaseOrder.Undo',
}
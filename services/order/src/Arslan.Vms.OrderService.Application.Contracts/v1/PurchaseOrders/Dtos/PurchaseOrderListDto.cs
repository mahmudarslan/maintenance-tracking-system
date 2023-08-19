using Arslan.Vms.OrderService.PurchaseOrders;
using System;
using System.Collections.Generic;

namespace Arslan.Vms.OrderService.v1.PurchaseOrders.Dtos
{
    public class PurchaseOrderListDto
    {
        public Guid Id { get; set; }
        public Guid? VendorId { get; set; }
        public Guid? LocationId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreationTime { get; set; }
        public PurchaseOrderPaymentStatus PaymentStatus { get; set; }
        public PurchaseOrderInventoryStatus InventoryStatus { get; set; }
        public string OrderNumber { get; set; }
        public string OrderRemarks { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get; set; }
        public decimal Total { get; set; }
        public string VendorName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
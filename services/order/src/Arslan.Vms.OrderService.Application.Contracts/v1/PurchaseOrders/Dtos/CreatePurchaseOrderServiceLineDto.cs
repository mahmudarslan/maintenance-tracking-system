using System;

namespace Arslan.Vms.OrderService.v1.PurchaseOrders.Dtos
{
    public class CreatePurchaseOrderServiceLineDto
    {
        public Guid ProductId { get; set; }
        public Guid TaxCodeId { get; set; }
        public int LineNum { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public bool ServiceCompleted { get; set; }
        public bool DiscountIsPercent { get; set; }
    }
}
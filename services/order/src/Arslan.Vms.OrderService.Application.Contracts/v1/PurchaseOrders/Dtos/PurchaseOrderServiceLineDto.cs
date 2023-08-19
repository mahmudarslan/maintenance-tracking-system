using System;

namespace Arslan.Vms.OrderService.v1.PurchaseOrders.Dtos
{
    public class PurchaseOrderServiceLineDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Description { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public int LineNum { get; set; }
        public bool ServiceCompleted { get; set; }
    }
}
using System;

namespace Arslan.Vms.OrderService.v1.SalesOrders.Dtos
{
    public class SalesOrderServiceLineDto
    {
        public Guid Id { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? TechnicianId { get; set; }
        public Guid? TaxCodeId { get; set; }
        //public int Version { get; set; }
        public string ProductName { get; set; }
        public bool ProductIsDeleted { get; set; }
        public int LineNum { get; set; }
        public string Description { get; set; }
        public decimal Discount { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public bool DiscountIsPercent { get; set; }
    }
}
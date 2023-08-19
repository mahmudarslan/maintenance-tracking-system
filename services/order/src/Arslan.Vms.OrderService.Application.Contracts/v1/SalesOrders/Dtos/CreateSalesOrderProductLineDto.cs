using System;

namespace Arslan.Vms.OrderService.v1.SalesOrders.Dtos
{
    public class CreateSalesOrderProductLineDto
    {
        public Guid LocationId { get; set; }
        public Guid ProductId { get; set; }
        public Guid TaxCodeId { get; set; }
        public DateTime PickDate { get; set; }
        public int LineNum { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public bool DiscountIsPercent { get; set; }
    }
}
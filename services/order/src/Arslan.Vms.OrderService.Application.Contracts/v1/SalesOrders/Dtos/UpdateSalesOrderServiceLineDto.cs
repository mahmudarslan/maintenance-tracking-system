using System;

namespace Arslan.Vms.OrderService.v1.SalesOrders.Dtos
{
    public class UpdateSalesOrderServiceLineDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid TaxCodeId { get; set; }
        public Guid? TechnicianId { get; set; }
        public int LineNum { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public bool ServiceCompleted { get; set; }
        public bool DiscountIsPercent { get; set; }
        //public bool IsDeleted { get; set; }
    }
}
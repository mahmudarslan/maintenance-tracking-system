using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.SalesOrders.Versions
{
    public class SalesOrderLineVersion : BasicAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid SalesOrderLineId { get; set; }

        public virtual Guid? TenantId { get; set; }
        public virtual Guid SalesOrderId { get; set; }
        public virtual int Version { get; set; }
        public virtual int LineNum { get; set; }
        public virtual string Description { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal Quantity { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal UnitPrice { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal Discount { get; set; }
        [Column(TypeName = "decimal(21, 5)")]
        public virtual decimal SubTotal { get; set; }
        [StringLength(20)]
        public virtual string QuantityUom { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal QuantityDisplay { get; set; }
        public virtual bool DiscountIsPercent { get; set; }
        public virtual Guid ProductId { get; set; }
        public virtual Guid? TaxCodeId { get; set; }
        [Column(TypeName = "decimal(10,5)")]
        public virtual decimal Tax1Rate { get; set; }
        [Column(TypeName = "decimal(10,5)")]
        public virtual decimal Tax2Rate { get; set; }
        public virtual bool? ServiceCompleted { get; set; }
        public virtual Guid? TechnicianId { get; set; }
    }
}
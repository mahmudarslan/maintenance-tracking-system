using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.SalesOrders.Versions
{
    public class SalesOrderPickLineVersion : BasicAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid SalesOrderPickLineId { get; set; }

        public virtual Guid? TenantId { get; set; }
        public virtual Guid SalesOrderId { get; set; }
        public virtual int Version { get; set; }
        public virtual int LineNum { get; set; }
        public virtual string Description { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal Quantity { get; set; }
        public virtual Guid LocationId { get; set; }
        [StringLength(100)]
        public string Sublocation { get; set; }
        [StringLength(20)]
        public virtual string QuantityUom { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal QuantityDisplay { get; set; }
        public virtual Guid ProductId { get; set; }
        public virtual DateTime PickDate { get; set; }
    }
}
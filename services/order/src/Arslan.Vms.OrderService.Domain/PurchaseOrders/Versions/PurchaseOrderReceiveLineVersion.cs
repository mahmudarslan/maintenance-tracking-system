using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.PurchaseOrders.Versions
{
    public class PurchaseOrderReceiveLineVersion : BasicAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid PurchaseOrderReceiveLineId { get; set; }


        public virtual Guid? TenantId { get; set; }
        public virtual Guid PurchaseOrderId { get; set; }
        public virtual int Version { get; set; }
        public virtual DateTime ReceiveDate { get; set; }
        public virtual string Description { get; set; }
        [StringLength(100)]
        public virtual string VendorLineCode { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal Quantity { get; set; }
        public virtual Guid LocationId { get; set; }
        public virtual string Sublocation { get; set; }
        public virtual string QuantityUom { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal QuantityDisplay { get; set; }
        public virtual Guid ProductId { get; set; }
        public virtual int LineNum { get; set; }
    }
}
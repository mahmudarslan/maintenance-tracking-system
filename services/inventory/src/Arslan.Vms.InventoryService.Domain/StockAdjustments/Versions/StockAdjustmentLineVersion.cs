using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.InventoryService.StockAdjustments.Versions
{
    public class StockAdjustmentLineVersion : BasicAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid StockAdjustmentLineId { get; set; }


        public virtual Guid? TenantId { get; set; }
        public virtual int Version { get; set; }
        public virtual Guid StockAdjustmentId { get; set; }
        public virtual Guid ProductId { get; set; }
        public virtual string Description { get; set; }
        public virtual Guid LocationId { get; set; }
        [StringLength(100)]
        public string Sublocation { get; protected set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal QuantityBefore { get; set; }
        [StringLength(20)]
        public virtual string QuantityBeforeUom { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal QuantityBeforeDisplay { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal QuantityAfter { get; set; }
        [StringLength(20)]
        public virtual string QuantityAfterUom { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal QuantityAfterDisplay { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal Difference { get; set; }
        [StringLength(20)]
        public virtual string DifferenceUom { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal DifferenceDisplay { get; set; }
        //public virtual string Serials { get; set; }

    }
}
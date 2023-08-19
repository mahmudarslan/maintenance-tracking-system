//using Arslan.Vms.Base.Products;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.InventoryService.StockAdjustments
{
    public class StockAdjustmentLine : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual int Version { get; protected set; }
        public virtual Guid StockAdjustmentId { get; protected set; }
        public virtual Guid ProductId { get; protected set; }
        public virtual string Description { get; set; }
        public virtual Guid LocationId { get; protected set; }
        [StringLength(100)]
        public string Sublocation { get; protected set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal QuantityBefore { get; protected set; }
        [StringLength(20)]
        public virtual string QuantityBeforeUom { get; protected set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal QuantityBeforeDisplay { get; protected set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal QuantityAfter { get; protected set; }
        [StringLength(20)]
        public virtual string QuantityAfterUom { get; protected set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal QuantityAfterDisplay { get; protected set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal Difference { get; set; }
        [StringLength(20)]
        public virtual string DifferenceUom { get; protected set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal DifferenceDisplay { get; protected set; }
        //public virtual string Serials { get; set; }

        //[NotMapped]
        //public virtual Product Product { get; set; }

        protected StockAdjustmentLine()
        {

        }

        public StockAdjustmentLine(Guid id, Guid? tenantId, Guid stockAdjustmentId, Guid productId,
            Guid locationId, int version, decimal quantityBefore, decimal quantityAfter) : base(id)
        {
            TenantId = tenantId;
            Version = version;
            StockAdjustmentId = stockAdjustmentId;
            ProductId = productId;
            LocationId = locationId;
            QuantityBefore = quantityBefore;
            QuantityAfter = quantityAfter;
            SetQuantity(quantityBefore, quantityAfter);
        }

        public virtual void SetQuantity(decimal oldValue, decimal newValue)
        {
            QuantityBefore = oldValue;
            QuantityBeforeDisplay = oldValue;

            QuantityAfter = newValue;
            QuantityAfterDisplay = newValue;

            Difference = newValue - oldValue;
            DifferenceDisplay = Difference;
        }

        public virtual void SetQuantity(decimal newValue)
        {
            QuantityBefore = QuantityAfter;
            QuantityBeforeDisplay = QuantityAfter;

            QuantityAfter = newValue;
            QuantityAfterDisplay = newValue;

            Difference = QuantityAfter - QuantityBefore;
            DifferenceDisplay = Difference;
        }

        public virtual void SubtractQuantity(decimal value)
        {
            var beforeValue = QuantityAfter;

            if (beforeValue != value)
            {
                QuantityAfter = beforeValue - Math.Abs(value);
                QuantityAfterDisplay = QuantityAfter;

                QuantityBefore = beforeValue;
                QuantityBeforeDisplay = beforeValue;

                Difference = QuantityAfter - Math.Abs(QuantityBefore);
                DifferenceDisplay = Difference;
            }
        }
    }
}
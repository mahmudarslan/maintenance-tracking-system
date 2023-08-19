using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.InventoryService.StockAdjustments.Versions
{
    public class StockAdjustmentVersion : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid StockAdjustmentId { get; set; }


        public virtual Guid? TenantId { get; set; }
        public virtual int Version { get; set; }
        [StringLength(30)]
        public virtual string AdjustmentNumber { get; set; }
        public virtual string Remarks { get; set; }
        public virtual bool IsCancelled { get; set; }

        [NotMapped]
        public virtual List<StockAdjustmentLineVersion> Lines { get; set; }
    }
}
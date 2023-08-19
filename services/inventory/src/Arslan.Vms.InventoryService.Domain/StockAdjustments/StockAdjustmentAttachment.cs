using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.InventoryService.StockAdjustments
{
    public class StockAdjustmentAttachment : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }
        public virtual Guid StockAdjustmentId { get; set; }
        public virtual Guid FileAttachmentId { get; set; }
    }
}
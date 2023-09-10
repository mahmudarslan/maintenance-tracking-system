using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.PurchaseOrders
{
    public class PurchaseOrderAttachment : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }
        public virtual Guid PurchaseOrderId { get; set; }
        public virtual Guid FileAttachmentId { get; set; }

        protected PurchaseOrderAttachment()
        {

        }

        public PurchaseOrderAttachment(Guid id, Guid? tenantId, Guid purchaseOrderId, Guid fileAttachmentId) : base(id)
        {
            TenantId = tenantId;
            PurchaseOrderId = purchaseOrderId;
            FileAttachmentId = fileAttachmentId;
        }
    }
}
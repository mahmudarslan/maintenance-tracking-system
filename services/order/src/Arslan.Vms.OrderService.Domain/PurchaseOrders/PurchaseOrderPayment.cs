using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.PurchaseOrders
{
    public class PurchaseOrderPayment : FullAuditedEntity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }
        public virtual Guid PurchaseOrderId { get; set; }
        public virtual Guid PaymentId { get; set; }
    }
}
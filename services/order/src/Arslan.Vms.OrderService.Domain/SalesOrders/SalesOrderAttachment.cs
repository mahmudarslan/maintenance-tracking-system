using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.SalesOrders
{
    public class SalesOrderAttachment : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }
        public virtual Guid SalesOrderId { get; set; }
        public virtual Guid FileAttachmentId { get; set; }

        protected SalesOrderAttachment()
        {

        }

        public SalesOrderAttachment(Guid id, Guid? tenantId, Guid salesOrderId, Guid fileAttachmentId) : base(id)
        {
            TenantId = tenantId;
            SalesOrderId = salesOrderId;
            FileAttachmentId = fileAttachmentId;
        }
    }
}
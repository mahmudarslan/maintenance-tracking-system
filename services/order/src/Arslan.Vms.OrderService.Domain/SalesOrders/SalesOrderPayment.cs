using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.SalesOrders
{
    public class SalesOrderPayment : FullAuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public Guid SalesOrderId { get; set; }
        public Guid PaymentId { get; set; }
    }
}
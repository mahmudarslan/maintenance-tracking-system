using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.IdentityService.Customers
{
    public class CustomerPayment : BasicAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }
        public virtual Guid PaymentId { get; set; }
        public virtual Guid CustomerId { get; set; }
    }
}
using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.Vendors
{
    public class VendorPayment : BasicAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }
        public virtual Guid PaymentId { get; set; }
        public virtual Guid VendorId { get; set; }
    }
}
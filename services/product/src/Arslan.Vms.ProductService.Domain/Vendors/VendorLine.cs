using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.Vendors
{
    public class VendorLine : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid VendorId { get; protected set; }
        public virtual Guid ProductId { get; protected set; }
        public virtual string VendorItemCode { get; protected set; }
        public virtual decimal Cost { get; protected set; }

        protected VendorLine() { }

        public VendorLine(Guid id, Guid? tenantId, Guid vendorId, Guid productId, string vendorItemCode, decimal cost) : base(id)
        {
            TenantId = tenantId;
            VendorId = vendorId;
            ProductId = productId;
            VendorItemCode = vendorItemCode;
            Cost = cost;
        }
    }
}
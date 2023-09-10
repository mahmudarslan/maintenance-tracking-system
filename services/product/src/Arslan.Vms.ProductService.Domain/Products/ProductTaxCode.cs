using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.Products
{
    public class ProductTaxCode : FullAuditedEntity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual int Version { get; protected set; }
        public virtual Guid ProductId { get; protected set; }
        public virtual Guid TaxingSchemeId { get; protected set; }
        public virtual Guid? TaxCodeId { get; protected set; }

        protected ProductTaxCode() { }

        public ProductTaxCode(Guid id, Guid? tenantId, Guid productId, Guid taxingSchemeId, Guid? taxCodeId) : base(id)
        {
            TenantId = tenantId;
            ProductId = productId;
            TaxingSchemeId = taxingSchemeId;
            TaxCodeId = taxCodeId;
        }
    }
}
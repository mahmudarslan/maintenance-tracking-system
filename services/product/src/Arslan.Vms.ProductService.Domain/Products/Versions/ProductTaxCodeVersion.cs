using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.Products.Versions
{
    public class ProductTaxCodeVersion : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }
        public virtual int Version { get; set; }
        public virtual Guid ProductId { get; set; }
        public virtual Guid TaxingSchemeId { get; set; }
        public virtual Guid? TaxCodeId { get; set; }

        protected ProductTaxCodeVersion() { }

        public ProductTaxCodeVersion(Guid id, ProductTaxCode p) : base(id)
        {
            TenantId = p.TenantId;
            Version = p.Version;
            ProductId = p.ProductId;
            TaxingSchemeId = p.TaxingSchemeId;
            TaxCodeId = p.TaxCodeId;
        }
    }
}
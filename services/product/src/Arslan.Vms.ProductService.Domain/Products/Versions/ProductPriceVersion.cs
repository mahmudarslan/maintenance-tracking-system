using System;
using Arslan.Vms.ProductService.Pricing;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.Products.Versions
{
    public class ProductPriceVersion : FullAuditedEntity<Guid>, IMultiTenant
    {
        public virtual Guid ProductPriceId { get; protected set; }
        public virtual Guid? TenantId { get; protected set; }
        public virtual int Version { get; protected set; }
        public virtual Guid PricingSchemeId { get; protected set; }
        public virtual decimal UnitPrice { get; protected set; }
        public virtual Guid ProductId { get; protected set; }
        public virtual PriceType ItemPriceType { get; protected set; }
        public virtual decimal FixedMarkup { get; protected set; }
        public virtual decimal StdUomPrice { get; protected set; }
        public virtual decimal PoUomPrice { get; protected set; }

        protected ProductPriceVersion() { }

        public ProductPriceVersion(Guid id, ProductPrice p) : base(id)
        {
            ProductPriceId = p.Id;
            TenantId = p.TenantId;
            Version = p.Version;
            PricingSchemeId = p.PricingSchemeId;
            UnitPrice = p.UnitPrice;
            ProductId = p.ProductId;
            ItemPriceType = p.ItemPriceType;
            FixedMarkup = p.FixedMarkup;
            StdUomPrice = p.StdUomPrice;
            PoUomPrice = p.PoUomPrice;
        }
    }
}
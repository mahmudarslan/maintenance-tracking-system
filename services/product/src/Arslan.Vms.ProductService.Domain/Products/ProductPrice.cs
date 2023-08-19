using System;
using System.Linq;
using Arslan.Vms.ProductService.Pricing;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.Products
{
    public class ProductPrice : FullAuditedEntity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual int Version { get; protected set; }
        public virtual Guid PricingSchemeId { get; protected set; }
        public virtual decimal UnitPrice { get; protected set; }
        public virtual Guid ProductId { get; protected set; }
        public virtual PriceType ItemPriceType { get; protected set; }
        public virtual decimal FixedMarkup { get; protected set; }
        public virtual decimal StdUomPrice { get; set; }
        public virtual decimal PoUomPrice { get; set; }

        protected ProductPrice() { }

        public ProductPrice(Guid id, Guid? tenantId, int version, Guid pricingSchemeId,
            decimal unitPrice, Guid productId, PriceType itemPriceType, decimal fixedMarkup) : base(id)
        {

            if (pricingSchemeId == Guid.Empty)
            {
                throw new Exception("pricingSchemeId is not empty");
            }

            //if (!Enum.GetValues<PriceType>().Contains(itemPriceType))
            //{
            //    throw new Exception("itemPriceType not contain value in PriceType");
            //}

            TenantId = tenantId;
            Version = version;
            PricingSchemeId = pricingSchemeId;
            UnitPrice = unitPrice;
            ProductId = productId;
            ItemPriceType = itemPriceType;
            FixedMarkup = fixedMarkup;
        }

        public void Set(Guid pricingSchemeId, decimal unitPrice, Guid productId, PriceType itemPriceType, decimal fixedMarkup)
        {
            if (pricingSchemeId == Guid.Empty)
            {
                throw new Exception("pricingSchemeId is not empty");
            }

            //if (!Enum.GetValues<PriceType>().Contains(itemPriceType))
            //{
            //    throw new Exception("itemPriceType not contain value in PriceType");
            //}

            PricingSchemeId = pricingSchemeId;
            UnitPrice = unitPrice;
            ProductId = productId;
            ItemPriceType = itemPriceType;
            FixedMarkup = fixedMarkup;
        }

        public void SetVersion(int version)
        {
            Version = version;
        }
    }
}
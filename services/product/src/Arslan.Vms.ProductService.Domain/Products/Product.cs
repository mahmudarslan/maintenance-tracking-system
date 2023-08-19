using System;
using System.Collections.Generic;
using System.Linq;
using Arslan.Vms.ProductService.Pricing;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.Products
{
    public class Product : FullAuditedAggregateRoot<Guid>, IMultiTenant, IProduct
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual int Version { get; protected set; }
        public virtual ProductType ProductType { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Description { get; set; }
        public virtual string Remarks { get; set; }
        public virtual string Barcode { get; set; }
        public virtual Guid ProductCategoryId { get; protected set; }
        public virtual Guid? DefaultLocationId { get; set; }
        public virtual string DefaultSublocation { get; set; }
        public virtual decimal ReorderPoint { get; set; }
        public virtual decimal ReorderQuantity { get; set; }
        public virtual string Uom { get; set; }
        public virtual decimal MasterPackQty { get; set; }
        public virtual decimal InnerPackQty { get; set; }
        public virtual decimal CaseLenght { get; set; }
        public virtual decimal CaseWidth { get; set; }
        public virtual decimal CaseHeight { get; set; }
        public virtual decimal CaseWeight { get; set; }
        public virtual decimal ProductLenght { get; set; }
        public virtual decimal ProductWidth { get; set; }
        public virtual decimal ProductHeight { get; set; }
        public virtual decimal ProductWeight { get; set; }
        public virtual Guid? LastVendorId { get; set; }
        public virtual bool IsSellable { get; set; }
        public virtual bool IsPurchaseable { get; set; }
        public virtual Guid? PictureFileAttachmentId { get; set; }
        public virtual string SoUomName { get; set; }
        public virtual decimal SoUomRatioStd { get; set; }
        public virtual decimal SoUomRatio { get; set; }
        public virtual string PoUomName { get; set; }
        public virtual decimal PoUomRatioStd { get; set; }
        public virtual decimal PoUomRatio { get; set; }
        public virtual decimal UnitCost { get; set; }
        public virtual ICollection<ProductAttachment> Attachments { get; protected set; }
        public virtual ICollection<ProductPrice> Prices { get; protected set; }
        public virtual ICollection<ProductTaxCode> TaxCodes { get; protected set; }

        protected Product() { }

        public Product(Guid id, Guid? tenantId, ProductType productType, string name, Guid productCategoryId) : base(id)
        {
            TenantId = tenantId;
            Version = 1;
            ProductType = productType;
            Name = name;
            ProductCategoryId = productCategoryId;

            Attachments = new List<ProductAttachment>();
            Prices = new List<ProductPrice>();
            TaxCodes = new List<ProductTaxCode>();
        }

        public void SetProduct(ProductType productType, string name, Guid productCategoryId)
        {
            ProductType = productType;
            Name = name;
            ProductCategoryId = productCategoryId;
        }

        public void SetCategory(Guid categoryId)
        {
            ProductCategoryId = categoryId;
        }

        public void SetActiveted(bool value)
        {
            IsDeleted = !value;
            //IsCancelled = value;
        }

        public void IncreaseVersion()
        {
            Version += 1;

            foreach (var item in Prices)
            {
                item.SetVersion(Version);
            }
        }

        public void AddAttachment(Guid id, Guid fileAttachmentId)
        {
            var attachment = new ProductAttachment(TenantId, Id, fileAttachmentId);
            Attachments.Add(attachment);
        }

        public void AddPrice(Guid id, Guid pricingSchemeId, decimal unitPrice, PriceType itemPriceType, decimal fixedMarkup)
        {
            Prices.Add(new ProductPrice(id, TenantId, Version, pricingSchemeId, unitPrice, Id, itemPriceType, fixedMarkup));
        }

        public void AddTax(Guid id, Guid taxingSchemeId, Guid? taxCodeId)
        {
            TaxCodes.Add(new ProductTaxCode(id, TenantId, Id, taxingSchemeId, taxCodeId));
        }

        public void SetPrices(List<ProductPrice> prices)
        {
            Prices = prices;
        }

        public void SetPrice(Guid id, Guid pricingSchemeId, decimal unitPrice, PriceType itemPriceType, decimal fixedMarkup)
        {
            var price = Prices.FirstOrDefault(f => f.Id == id);
            price.Set(pricingSchemeId, unitPrice, Id, itemPriceType, fixedMarkup);
        }


        //private bool ProductCheckName(string name)
        //{
        //    return _productRepository.FirstOrDefault(f => f.Name == name) != null;
        //}

        //public void CheckName(string name)
        //{
        //    Check.NotNullOrEmpty(name, nameof(name));

        //    var productName = _productRepository.FirstOrDefault(f => f.Name == name);
        //    var ProductCheckName = productName != null;

        //    if (name != productName?.Name && ProductCheckName)
        //    {
        //        throw new UserFriendlyException("Same Name Product", "", "", null, Microsoft.Extensions.Logging.LogLevel.Error);
        //    }
        //}


        //public void CheckPolicy([NotNull] IProductPolicy policy)
        //{
        //    policy.CheckName(Name);
        //}


        //public ProductTag ProductTag(Guid? serial = null)
        //{
        //    return new ProductTag(TenantId, Id);
        //}

        //public ProductTag ProductTag(Guid productTagId, Guid? serial = null)
        //{
        //    return new ProductTag(productTagId, TenantId, Id);
        //}
    }
}
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.Products.Versions
{
    public class ProductVersion : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid ProductId { get; set; }
        public virtual Guid? TenantId { get; set; }
        public virtual int Version { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string Remarks { get; set; }
        public virtual string Barcode { get; set; }
        public virtual Guid ProductCategoryId { get; set; }
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

        protected ProductVersion() { }

        public ProductVersion(Guid id, Product p) : base(id)
        {
            ProductId = p.Id;
            TenantId = p.TenantId;
            Version = p.Version;
            ProductType = p.ProductType;
            Name = p.Name;
            Description = p.Description;
            Remarks = p.Remarks;
            Barcode = p.Barcode;
            ProductCategoryId = p.ProductCategoryId;
            DefaultLocationId = p.DefaultLocationId;
            DefaultSublocation = p.DefaultSublocation;
            ReorderPoint = p.ReorderPoint;
            ReorderQuantity = p.ReorderQuantity;
            Uom = p.Uom;
            MasterPackQty = p.MasterPackQty;
            InnerPackQty = p.InnerPackQty;
            CaseLenght = p.CaseLenght;
            CaseWidth = p.CaseWidth;
            CaseHeight = p.CaseHeight;
            CaseWeight = p.CaseWeight;
            ProductLenght = p.ProductLenght;
            ProductWidth = p.ProductWidth;
            ProductHeight = p.ProductHeight;
            ProductWeight = p.ProductWeight;
            LastVendorId = p.LastVendorId;
            IsSellable = p.IsSellable;
            IsPurchaseable = p.IsPurchaseable;
            PictureFileAttachmentId = p.PictureFileAttachmentId;
            SoUomName = p.SoUomName;
            SoUomRatioStd = p.SoUomRatioStd;
            PoUomName = p.PoUomName;
            PoUomRatioStd = p.PoUomRatioStd;
            PoUomRatio = p.PoUomRatio;
            UnitCost = p.UnitCost;
        }
    }
}
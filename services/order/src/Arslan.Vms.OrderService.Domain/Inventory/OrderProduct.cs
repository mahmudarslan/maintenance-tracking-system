//using Arslan.Vms.Base.Products;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Arslan.Vms.OrderService.Inventory
{
    public class OrderProduct : FullAuditedAggregateRoot<Guid>//, IProduct
    {
        public virtual Guid? TenantId { get; set; }
        public virtual int Version { get; set; }
        public virtual int ProductType { get; set; }
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
    }
}
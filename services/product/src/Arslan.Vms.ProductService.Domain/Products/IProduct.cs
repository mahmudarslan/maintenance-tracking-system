using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.Products
{
    public interface IProduct : IAggregateRoot<Guid>, IMultiTenant
    {
        int Version { get; }
        ProductType ProductType { get; }
        string Name { get; }
        string Description { get; }
        string Remarks { get; }
        string Barcode { get; }
        Guid ProductCategoryId { get; }
        Guid? DefaultLocationId { get; }
        string DefaultSublocation { get; }
        decimal ReorderPoint { get; }
        decimal ReorderQuantity { get; }
        string Uom { get; }
        decimal MasterPackQty { get; }
        decimal InnerPackQty { get; }
        decimal CaseLenght { get; }
        decimal CaseWidth { get; }
        decimal CaseHeight { get; }
        decimal CaseWeight { get; }
        decimal ProductLenght { get; }
        decimal ProductWidth { get; }
        decimal ProductHeight { get; }
        decimal ProductWeight { get; }
        Guid? LastVendorId { get; }
        bool IsSellable { get; }
        bool IsPurchaseable { get; }
        Guid? PictureFileAttachmentId { get; }
        string SoUomName { get; set; }
        decimal SoUomRatioStd { get; }
        decimal SoUomRatio { get; }
        string PoUomName { get; }
        decimal PoUomRatioStd { get; }
        decimal PoUomRatio { get; }
        //bool TrackSerials { get; set; }

        public decimal UnitCost { get; }

        //Guid? StandartUoMId { get; set; }
        //Guid? SalesUoMId { get; set; }
        //Guid? PurchasingUoMId { get; set; }
        //decimal Cost { get; set; }
    }
}
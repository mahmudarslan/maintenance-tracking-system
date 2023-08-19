using Arslan.Vms.ProductService.Products;
using System;

namespace Arslan.Vms.ProductService.v1.Products.Dtos
{
    public class ProductItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductCategoryId { get; set; }
        public ProductType ProductType { get; set; }
        public Guid? DefaultLocationId { get; set; }
        public string Name { get; set; }
        public decimal UnitCost { get; set; }
        public decimal NormalPrice { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }
    }
}
using Arslan.Vms.ProductService.Products;
using Arslan.Vms.ProductService.v1.Files.Dtos;
using System;
using System.Collections.Generic;

namespace Arslan.Vms.ProductService.v1.Products.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public Guid ProductCategoryId { get; set; }
        public Guid? DefaultLocationId { get; set; }
        public ProductType ProductType { get; set; }
        public DateTime CreationTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public decimal UnitCost { get; set; }
        public string Barcode { get; set; }
        public decimal ReorderPoint { get; set; }
        public int Version { get; set; }
        public bool IsDeleted { get; set; }

        public ProductDto()
        {
            Prices = new List<ProductPriceDto>();
            Attachments = new List<FileAttachmentDto>();
        }

        public List<FileAttachmentDto> Attachments { get; set; }
        public List<ProductPriceDto> Prices { get; set; }
    }
}
using Arslan.Vms.ProductService.Products;
using Arslan.Vms.ProductService.v1.Files.Dtos;
using System;
using System.Collections.Generic;

namespace Arslan.Vms.ProductService.v1.Products.Dtos
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public ProductType ProductType { get; set; }
        public Guid ProductCategoryId { get; set; }
        public Guid? DefaultLocationId { get; set; }
        public Guid? FakeId { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public decimal UnitCost { get; set; }
        public string Barcode { get; set; }
        public decimal ReorderPoint { get; set; }
        public List<FileAttachmentDto> Files { get; set; }
        public List<CreateUpdateProductPriceDto> Prices { get; set; }

        public UpdateProductDto()
        {
            Prices = new List<CreateUpdateProductPriceDto>();
            Files = new List<FileAttachmentDto>();
        }
    }
}
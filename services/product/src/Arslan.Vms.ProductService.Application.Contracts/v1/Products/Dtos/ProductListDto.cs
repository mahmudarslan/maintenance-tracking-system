using Arslan.Vms.ProductService.Products;
using System;

namespace Arslan.Vms.ProductService.v1.Products.Dtos
{
    public class ProductListDto
    {
        public Guid Id { get; set; }
        public Guid ProductCategoryId { get; set; }
        public ProductType ProductType { get; set; }
        public DateTime CreationTime { get; set; }
        public string Name { get; set; }
        public decimal NormalPrice { get; set; }
        public string CategoryName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
using System;

namespace Arslan.Vms.ProductService.v1.Categories
{
    public class CreateCategoryDto
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
    }
}
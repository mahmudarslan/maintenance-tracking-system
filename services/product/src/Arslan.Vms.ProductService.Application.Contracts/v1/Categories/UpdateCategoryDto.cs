using System;

namespace Arslan.Vms.ProductService.v1.Categories
{
    public class UpdateCategoryDto
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
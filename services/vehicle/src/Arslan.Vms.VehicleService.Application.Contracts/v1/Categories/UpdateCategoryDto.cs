using System;

namespace Arslan.Vms.VehicleService.v1.Categories
{
    public class UpdateCategoryDto
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
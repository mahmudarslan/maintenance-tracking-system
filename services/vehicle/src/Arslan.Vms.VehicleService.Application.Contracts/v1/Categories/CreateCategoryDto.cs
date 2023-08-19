using System;

namespace Arslan.Vms.VehicleService.v1.Categories
{
    public class CreateCategoryDto
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
    }
}
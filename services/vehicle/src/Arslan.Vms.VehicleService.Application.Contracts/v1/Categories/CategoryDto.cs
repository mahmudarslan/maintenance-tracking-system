using System;

namespace Arslan.Vms.VehicleService.v1.Categories
{
    public class CategoryDto
    {
        public Guid? ParentId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool IsDeleted { get; set; }
    }
}
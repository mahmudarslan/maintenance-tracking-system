using System;

namespace Arslan.Vms.ProductService.v1.Locations.Dtos
{
    public class LocationDto
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public int Level { get; set; }
    }
}
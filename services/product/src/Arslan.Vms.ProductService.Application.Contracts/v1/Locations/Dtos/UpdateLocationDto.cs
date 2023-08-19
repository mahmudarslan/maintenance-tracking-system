using System;

namespace Arslan.Vms.ProductService.v1.Locations.Dtos
{
    public class UpdateLocationDto
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
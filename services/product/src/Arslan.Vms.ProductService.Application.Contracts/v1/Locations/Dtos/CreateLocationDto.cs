using System;

namespace Arslan.Vms.ProductService.v1.Locations.Dtos
{
    public class CreateLocationDto
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
    }
}
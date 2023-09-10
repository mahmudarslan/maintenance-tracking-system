using System;

namespace Arslan.Vms.VehicleService.v1.Locations.Dtos
{
    public class CreateLocationDto
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
    }
}
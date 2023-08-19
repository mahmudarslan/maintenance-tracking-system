using System;

namespace Arslan.Vms.VehicleService.v1.Locations.Dtos
{
    public class UpdateLocationDto
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
using System;

namespace Arslan.Vms.VehicleService.v1.Vehicles.Dtos
{
    public class UpdateVehicleDto
    {
        public Guid Id { get; set; }
        public Guid ModelId { get; set; }
        //public Guid UserId { get; set; }
        //public Guid UserVehicleId { get; set; }
        public string Plate { get; set; }
        public string Color { get; set; }
        public string Motor { get; set; }
        public string Chassis { get; set; }
        public bool IsDeleted { get; set; }
    }
}
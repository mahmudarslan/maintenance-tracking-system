using System;

namespace Arslan.Vms.VehicleService.v1.Vehicles.Dtos
{
    public class VehicleDto
    {
        public Guid Id { get; set; }
        public Guid ModelId { get; set; }
        public string Plate { get; set; }
        public string Color { get; set; }
        public string Motor { get; set; }
        public string Chassis { get; set; }


        public Guid UserId { get; set; }
        public Guid BrandId { get; set; }
        //public Guid UserVehicleId { get; set; }
        //public string CustomerCN { get; set; }
        //public string BrandName { get; set; }
        //public string ModelName { get; set; }

        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
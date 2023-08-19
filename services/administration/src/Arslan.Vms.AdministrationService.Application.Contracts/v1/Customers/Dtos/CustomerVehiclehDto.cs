using System;

namespace Arslan.Vms.AdministrationService.v1.Customers.Dtos
{
    public class CustomerVehiclehDto
    {
        public Guid Id { get; set; }
        //public Guid UserVehicleId { get; set; }
        public Guid VehicleId { get; set; }
        public string Name { get; set; }
    }
}
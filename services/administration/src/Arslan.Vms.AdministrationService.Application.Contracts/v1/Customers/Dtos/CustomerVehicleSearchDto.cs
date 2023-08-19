using System;

namespace Arslan.Vms.AdministrationService.v1.Customers.Dtos
{
    public class CustomerVehicleSearchDto
    {
        public Guid Id { get; set; }
        //public Guid UserVehicleId { get; set; }
        public Guid VehicleId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string PlateNo { get; set; }
        public string SelectionField { get; set; }
    }
}
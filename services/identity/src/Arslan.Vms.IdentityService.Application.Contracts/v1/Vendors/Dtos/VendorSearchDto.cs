using System;

namespace Arslan.Vms.IdentityService.v1.Vendors.Dtos
{
    public class VendorSearchDto
    {
        public Guid Id { get; set; }
        public Guid UserVehicleId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string PlateNo { get; set; }
        public string SelectionField { get; set; }
    }
}
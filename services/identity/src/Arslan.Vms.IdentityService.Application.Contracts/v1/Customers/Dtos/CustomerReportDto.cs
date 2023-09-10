using System;
using System.Collections.Generic;

namespace Arslan.Vms.IdentityService.v1.Customers.Dtos
{
    public class CustomerReportDto
    {
        public string UserCn { get; set; }
        public string Address { get; set; }
        public string HomePhoneNumber { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string VehiclePlateNo { get; set; }
        public string VehicleColor { get; set; }
        public string VehicleChassis { get; set; }
        public string VehicleMotor { get; set; }
        public string VehicleBrandName { get; set; }
    }

}

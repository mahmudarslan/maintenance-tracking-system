using Arslan.Vms.IdentityService.v1.Addresses.Dtos;
using System;
using System.Collections.Generic;

namespace Arslan.Vms.IdentityService.v1.Employee.Dtos
{
    public class UpdateEmployeeDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string HomePhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string[] Roles { get; set; }
        public bool IsDeleted { get; set; }

        public List<UpdateAddressDto> Addresses { get; set; }
    }
}
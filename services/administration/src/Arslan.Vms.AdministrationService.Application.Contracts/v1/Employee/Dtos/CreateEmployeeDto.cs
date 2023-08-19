using Arslan.Vms.AdministrationService.v1.Addresses.Dtos;
using System;
using System.Collections.Generic;

namespace Arslan.Vms.AdministrationService.v1.Employee.Dtos
{
    public class CreateEmployeeDto
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
        public List<Guid> Roles { get; set; }

        public List<CreateAddressDto> Addresses { get; set; }
    }
}
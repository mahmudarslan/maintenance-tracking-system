﻿namespace Arslan.Vms.IdentityService.v1.Customers
{
    internal class IdentityUserUpdateDto
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
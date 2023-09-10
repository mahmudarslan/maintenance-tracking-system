﻿namespace Arslan.Vms.IdentityService.v1.Vendors
{
    internal class IdentityUserCreateDto
    {
        public IdentityUserCreateDto()
        {
        }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string[] RoleNames { get; set; }
    }
}
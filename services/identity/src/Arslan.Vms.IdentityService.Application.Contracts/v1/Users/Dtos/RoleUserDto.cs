using System;

namespace Arslan.Vms.IdentityService.v1.Users.Dtos
{
    public class RoleUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string NameSurname { get; set; }

    }
}
using System;

namespace Arslan.Vms.IdentityService.v1.Users.Dtos
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Value { get; set; }
    }
}
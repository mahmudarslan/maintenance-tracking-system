using System;

namespace Arslan.Vms.AdministrationService.v1.Users.Dtos
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Value { get; set; }
    }
}
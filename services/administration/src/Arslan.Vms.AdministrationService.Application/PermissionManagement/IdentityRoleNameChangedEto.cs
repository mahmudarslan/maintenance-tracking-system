using System;

namespace Arslan.Vms.AdministrationService.PermissionManagement;

[Serializable]
public class IdentityRoleNameChangedEto
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; }

    public string OldName { get; set; }
}
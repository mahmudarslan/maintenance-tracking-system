using System;
using System.Collections.Generic;

namespace Arslan.Vms.KeycloakService.Models;

[Serializable]
public class TenantConfig
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<ClientModel> Clients { get; set; } = new List<ClientModel>();
}

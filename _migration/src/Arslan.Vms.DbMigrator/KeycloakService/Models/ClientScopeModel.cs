using System;

namespace Arslan.Vms.KeycloakService.Models;

[Serializable]
public class ClientScopeModel
{
    public string Name { get; set; }
    public string RootUrl { get; set; }
}

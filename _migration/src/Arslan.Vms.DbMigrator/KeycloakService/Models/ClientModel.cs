using System;
using System.Collections.Generic;

namespace Arslan.Vms.KeycloakService.Models;

[Serializable]
public class ClientModel
{
    public string Name { get; set; }
    public string RootUrl { get; set; }
    public string Type { get; set; }
    public ClientSwaggerModel Swagger { get; set; }
    public List<string> RedirectUrls { get; set; } = new List<string>();
    public List<string> LogoutRedirectUrls { get; set; } = new List<string>();
    public List<string> WebOrigins { get; set; } = new List<string>();
    public List<ClientRoleModel> Roles { get; set; } = new List<ClientRoleModel>();
    public List<ClientScopeModel> Scopes { get; set; } = new List<ClientScopeModel>();
}

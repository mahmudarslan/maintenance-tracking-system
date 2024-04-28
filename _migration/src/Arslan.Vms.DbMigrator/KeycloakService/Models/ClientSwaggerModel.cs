using System;

namespace Arslan.Vms.KeycloakService.Models;

[Serializable]
public class ClientSwaggerModel
{
    public string Name { get; set; }
    public string SwaggerClientId { get; set; }
    public string SwaggerClientSecret { get; set; }
}
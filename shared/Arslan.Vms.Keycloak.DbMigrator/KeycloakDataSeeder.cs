using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Keycloak.Net;
using Keycloak.Net.Models.Clients;
using Keycloak.Net.Models.ClientScopes;
using Keycloak.Net.Models.ProtocolMappers;
using Keycloak.Net.Models.Roles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Arslan.Vms.DbMigrator;

public class KeyCloakDataSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly KeycloakClient _keycloakClient;
    private readonly KeycloakClientOptions _keycloakOptions;
    private readonly ILogger<KeyCloakDataSeeder> _logger;
    private readonly IConfiguration _configuration;

    public KeyCloakDataSeeder(IOptions<KeycloakClientOptions> keycloakClientOptions, ILogger<KeyCloakDataSeeder> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _keycloakOptions = keycloakClientOptions.Value;

        _keycloakClient = new KeycloakClient(
            _keycloakOptions.Url,
            _keycloakOptions.AdminUserName,
            _keycloakOptions.AdminPassword
        );
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await CreateClientScopesAsync();
        await CreateClientsAsync();
        await UpdateAdminUserAsync();
    }

    private async Task CreateClientScopesAsync()
    {
        await CreateScopeAsync(_configuration[$"Clients:AdministrationService:Name"]);
        await CreateScopeAsync(_configuration[$"Clients:IdentityService:Name"]);
        await CreateScopeAsync(_configuration[$"Clients:InventoryService:Name"]);
        await CreateScopeAsync(_configuration[$"Clients:OrderService:Name"]);
        await CreateScopeAsync(_configuration[$"Clients:PaymentService:Name"]);
        await CreateScopeAsync(_configuration[$"Clients:PlannerService:Name"]);
        await CreateScopeAsync(_configuration[$"Clients:ProductService:Name"]);
        await CreateScopeAsync(_configuration[$"Clients:VehicleService:Name"]);
        await CreateTenantScopeAsync(_configuration[$"Keycloak:realmName"], _configuration[$"Tenant:Id"]);
    }

    private async Task CreateScopeAsync(string scopeName)
    {
        var scope = (await _keycloakClient.GetClientScopesAsync(_keycloakOptions.RealmName))
            .FirstOrDefault(q => q.Name == scopeName);

        if (scope == null)
        {
            scope = new ClientScope
            {
                Name = scopeName,
                Description = scopeName + " scope",
                Protocol = "openid-connect",
                Attributes = new Attributes
                {
                    ConsentScreenText = scopeName,
                    DisplayOnConsentScreen = "true",
                    IncludeInTokenScope = "true"
                },
                ProtocolMappers = new List<ProtocolMapper>()
                {
                    new ProtocolMapper()
                    {
                        Name = scopeName,
                        Protocol = "openid-connect",
                        _ProtocolMapper = "oidc-audience-mapper",
                        Config =
                            new
                                Dictionary<string,
                                    string>()
                                {
                                    { "id.token.claim", "false" },
                                    { "access.token.claim", "true" },
                                    { "included.custom.audience", scopeName }
                                }
                    }
                }
            };

            await _keycloakClient.CreateClientScopeAsync(_keycloakOptions.RealmName, scope);
        }
    }

    private async Task CreateTenantScopeAsync(string scopeName, string tenantId)
    {
        var scope = (await _keycloakClient.GetClientScopesAsync(_keycloakOptions.RealmName))
            .FirstOrDefault(q => q.Name == scopeName);

        if (scope == null)
        {
            scope = new ClientScope
            {
                Name = _configuration[$"Tenant:Name"],
                Description = "Tenant " + scopeName + " scope",
                Protocol = "openid-connect",
                Attributes = new Attributes
                {
                    ConsentScreenText = scopeName,
                    DisplayOnConsentScreen = "true",
                    IncludeInTokenScope = "true"
                },
                ProtocolMappers = new List<ProtocolMapper>()
                {
                    new ProtocolMapper()
                    {
                        Name = "tenantid",
                        Protocol = "openid-connect",
                        _ProtocolMapper = "oidc-hardcoded-claim-mapper",
                        Config =
                            new
                                Dictionary<string,
                                    string>()
                                {
                                    { "access.token.claim", "true" },
                                    { "access.tokenResponse.claim", "false" },
                                    { "claim.name", "tenantid" },
                                    { "claim.value", tenantId },
                                    { "id.token.claim", "false" },
                                    { "userinfo.token.claim", "false" }
                                }
                    }
                }
            };

            try
            {
                await _keycloakClient.CreateClientScopeAsync(_keycloakOptions.RealmName, scope);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
    }

    private async Task CreateClientsAsync()
    {
        await CreateSwaggerClientAsync();
        await CreateWebClientAsync();
    }

    private async Task CreateWebClientAsync()
    {
        var webClient = (await _keycloakClient.GetClientsAsync(_keycloakOptions.RealmName, clientId: _configuration[$"Clients:Web:Name"]))
            .FirstOrDefault();

        if (webClient == null)
        {
            var webRootUrl = _configuration[$"Clients:Web:RootUrl"];
            var webGatewaySwaggerRootUrl = _configuration[$"Clients:WebGateway:RootUrl"].TrimEnd('/');
            var administrationServiceRootUrl = _configuration[$"Clients:AdministrationService:RootUrl"].TrimEnd('/');
            var identityServiceRootUrl = _configuration[$"Clients:IdentityService:RootUrl"].TrimEnd('/');
            var inventoryServiceRootUrl = _configuration[$"Clients:InventoryService:RootUrl"].TrimEnd('/');
            var orderServiceRootUrl = _configuration[$"Clients:OrderService:RootUrl"].TrimEnd('/');
            var paymentServiceRootUrl = _configuration[$"Clients:PaymentService:RootUrl"].TrimEnd('/');
            var plannerServiceRootUrl = _configuration[$"Clients:PlannerService:RootUrl"].TrimEnd('/');
            var productServiceRootUrl = _configuration[$"Clients:ProductService:RootUrl"].TrimEnd('/');
            var vehicleService = _configuration[$"Clients:VehicleService:RootUrl"].TrimEnd('/');

            webClient = new Client
            {
                ClientId = _configuration[$"Clients:Web:Name"],
                Name = "Vms Web Application",
                Protocol = "openid-connect",
                Enabled = true,
                BaseUrl = webRootUrl,
                RedirectUris = new List<string>
                {
                    $"{webRootUrl.TrimEnd('/')}*",
                    $"{webGatewaySwaggerRootUrl}/*",
                    $"{administrationServiceRootUrl}/*",
                    $"{identityServiceRootUrl}/*",
                    $"{inventoryServiceRootUrl}/*",
                    $"{orderServiceRootUrl}/*",
                    $"{paymentServiceRootUrl}/*",
                    $"{plannerServiceRootUrl}/*",
                    $"{productServiceRootUrl}/*",
                    $"{vehicleService}/*",
                },
                FrontChannelLogout = true,
                PublicClient = true
            };
            webClient.Attributes = new Dictionary<string, object>
            {
                { "post.logout.redirect.uris", $"{webRootUrl.TrimEnd('/')}" }
            };

            await _keycloakClient.CreateClientAsync(_keycloakOptions.RealmName, webClient);

            await AddOptionalScopesAsync(
                            _configuration[$"Clients:Web:Name"],
                            new List<string>
                            {
                                        _configuration[$"Clients:AdministrationService:Name"],
                                        _configuration[$"Clients:IdentityService:Name"],
                                        _configuration[$"Clients:InventoryService:Name"],
                                        _configuration[$"Clients:OrderService:Name"],
                                        _configuration[$"Clients:PaymentService:Name"],
                                        _configuration[$"Clients:PlannerService:Name"],
                                        _configuration[$"Clients:ProductService:Name"],
                                        _configuration[$"Clients:VehicleService:Name"]
                            });

            await AddDefaultClientScopesAsync(
                            _configuration[$"Clients:Web:Name"],
                            new List<string>
                            {
                                        _configuration[$"Tenant:Name"]
                            });

            await CreateRoleAsync("admin");
            await CreateRoleAsync("customer");
        }
    }

    private async Task CreateSwaggerClientAsync()
    {
        var swaggerClient =
            (await _keycloakClient.GetClientsAsync(_keycloakOptions.RealmName, clientId: _configuration[$"Swagger:SwaggerClientId"]))
            .FirstOrDefault();

        if (swaggerClient == null)
        {
            var webGatewaySwaggerRootUrl = _configuration[$"Clients:WebGateway:RootUrl"].TrimEnd('/');
            var administrationServiceRootUrl = _configuration[$"Clients:AdministrationService:RootUrl"].TrimEnd('/');
            var identityServiceRootUrl = _configuration[$"Clients:IdentityService:RootUrl"].TrimEnd('/');
            var inventoryServiceRootUrl = _configuration[$"Clients:InventoryService:RootUrl"].TrimEnd('/');
            var orderServiceRootUrl = _configuration[$"Clients:OrderService:RootUrl"].TrimEnd('/');
            var paymentServiceRootUrl = _configuration[$"Clients:PaymentService:RootUrl"].TrimEnd('/');
            var plannerServiceRootUrl = _configuration[$"Clients:PlannerService:RootUrl"].TrimEnd('/');
            var productServiceRootUrl = _configuration[$"Clients:ProductService:RootUrl"].TrimEnd('/');
            var vehicleService = _configuration[$"Clients:VehicleService:RootUrl"].TrimEnd('/');

            swaggerClient = new Client
            {
                ClientId = _configuration[$"Swagger:SwaggerClientId"],
                Name = _configuration[$"Swagger:Name"],
                Protocol = "openid-connect",
                Enabled = true,
                RedirectUris = new List<string>
                {
                    $"{webGatewaySwaggerRootUrl}/swagger/oauth2-redirect.html",
                    $"{administrationServiceRootUrl}/swagger/oauth2-redirect.html",
                    $"{identityServiceRootUrl}/swagger/oauth2-redirect.html",
                    $"{inventoryServiceRootUrl}/swagger/oauth2-redirect.html",
                    $"{orderServiceRootUrl}/swagger/oauth2-redirect.html",
                    $"{paymentServiceRootUrl}/swagger/oauth2-redirect.html",
                    $"{plannerServiceRootUrl}/swagger/oauth2-redirect.html",
                    $"{productServiceRootUrl}/swagger/oauth2-redirect.html",
                    $"{vehicleService}/swagger/oauth2-redirect.html"
                },
                FrontChannelLogout = true,
                PublicClient = false,
                ImplicitFlowEnabled = false,
                DirectAccessGrantsEnabled = false,
                StandardFlowEnabled = true,
                ServiceAccountsEnabled = false,
                SurrogateAuthRequired = false,
                Secret = _configuration[$"Swagger:SwaggerClientSecret"],
                BearerOnly = false,
                ClientAuthenticatorType = "client-secret",
            };

            await _keycloakClient.CreateClientAsync(_keycloakOptions.RealmName, swaggerClient);

            await AddOptionalScopesAsync(
                            _configuration[$"Swagger:SwaggerClientId"],
                            new List<string>
                            {
                                        _configuration[$"Clients:AdministrationService:Name"],
                                        _configuration[$"Clients:IdentityService:Name"],
                                        _configuration[$"Clients:InventoryService:Name"],
                                        _configuration[$"Clients:OrderService:Name"],
                                        _configuration[$"Clients:PaymentService:Name"],
                                        _configuration[$"Clients:PlannerService:Name"],
                                        _configuration[$"Clients:ProductService:Name"],
                                        _configuration[$"Clients:VehicleService:Name"]
                            });

            await AddDefaultClientScopesAsync(
                            _configuration[$"Swagger:SwaggerClientId"],
                            new List<string>
                            {
                                        _configuration[$"Tenant:Name"]
                            });
        }
    }

    private async Task AddDefaultClientScopesAsync(string clientName, List<string> scopes)
    {
        var client = (await _keycloakClient.GetClientsAsync(_keycloakOptions.RealmName, clientId: clientName))
            .FirstOrDefault();
        if (client == null)
        {
            _logger.LogError($"Couldn't find {clientName}! Could not seed optional scopes!");
            return;
        }

        var clientOptionalScopes =
            (await _keycloakClient.GetDefaultClientScopesAsync(_keycloakOptions.RealmName, client.Id)).ToList();

        var clientScopes = (await _keycloakClient.GetClientScopesAsync(_keycloakOptions.RealmName)).ToList();

        foreach (var scope in scopes)
        {
            if (!clientOptionalScopes.Any(q => q.Name == scope))
            {
                var serviceScope = clientScopes.First(q => q.Name == scope);
                _logger.LogInformation($"Seeding {scope} scope to {clientName}.");
                await _keycloakClient.UpdateDefaultClientScopeAsync(_keycloakOptions.RealmName, client.Id,
                    serviceScope.Id);
            }
        }
    }

    private async Task AddOptionalScopesAsync(string clientName, List<string> scopes)
    {
        var client = (await _keycloakClient.GetClientsAsync(_keycloakOptions.RealmName, clientId: clientName))
            .FirstOrDefault();
        if (client == null)
        {
            _logger.LogError($"Couldn't find {clientName}! Could not seed optional scopes!");
            return;
        }

        var clientOptionalScopes =
            (await _keycloakClient.GetOptionalClientScopesAsync(_keycloakOptions.RealmName, client.Id)).ToList();

        var clientScopes = (await _keycloakClient.GetClientScopesAsync(_keycloakOptions.RealmName)).ToList();

        foreach (var scope in scopes)
        {
            if (!clientOptionalScopes.Any(q => q.Name == scope))
            {
                var serviceScope = clientScopes.First(q => q.Name == scope);
                _logger.LogInformation($"Seeding {scope} scope to {clientName}.");
                await _keycloakClient.UpdateOptionalClientScopeAsync(_keycloakOptions.RealmName, client.Id,
                    serviceScope.Id);
            }
        }
    }

    public async Task CreateRoleAsync(string name)
    {
        var clientId = await GetClientIdAsync();

        try
        {
            var keycloakRole = await _keycloakClient.GetRoleByNameAsync(_keycloakOptions.RealmName, clientId, name);
        }
        catch (FlurlHttpException ex)
        {
            //if (ex.StatusCode == 404)
            {
                var role = new Role()
                {
                    Name = name,
                };
                await _keycloakClient.CreateRoleAsync(_keycloakOptions.RealmName, clientId, role);
            }
        }
    }

    public async Task<string> GetClientIdAsync()
    {
        var clients = _keycloakClient.GetClientsAsync(_keycloakOptions.RealmName, _configuration[$"Clients:Web:Name"]).Result;
        var _client = clients.FirstOrDefault(f => f.ClientId == _configuration[$"Clients:Web:Name"]);

        return await Task.FromResult(_client.Id);
    }

    private async Task CreateAdministrationClientAsync()
    {
        var administrationClient =
            (await _keycloakClient.GetClientsAsync(_keycloakOptions.RealmName,
                clientId: "Vms_AdministrationService"))
            .FirstOrDefault();

        if (administrationClient == null)
        {
            administrationClient = new Client()
            {
                ClientId = "Vms_AdministrationService",
                Name = "Administration service client",
                Protocol = "openid-connect",
                PublicClient = false,
                ImplicitFlowEnabled = false,
                AuthorizationServicesEnabled = false,
                StandardFlowEnabled = false,
                DirectAccessGrantsEnabled = false,
                ServiceAccountsEnabled = true,
                Secret = "1q2w3e*"
            };

            administrationClient.Attributes = new Dictionary<string, object>()
            {
                { "oauth2.device.authorization.grant.enabled", false },
                { "oidc.ciba.grant.enabled", false }
            };

            await _keycloakClient.CreateClientAsync(_keycloakOptions.RealmName, administrationClient);

            await AddDefaultClientScopesAsync(
                "Vms_AdministrationService",
                new List<string>
                {
                    "IdentityService"
                }
            );

            var insertedClient =
                (await _keycloakClient.GetClientsAsync(_keycloakOptions.RealmName, clientId: "Vms_AdministrationService"))
                .First();

            var clientIdProtocolMapper = insertedClient.ProtocolMappers.First(q => q.Name == "Client ID");

            clientIdProtocolMapper.Config["claim.name"] = "client_id";

            var result = await _keycloakClient.UpdateClientAsync(_keycloakOptions.RealmName, insertedClient.Id,
                insertedClient);
        }
    }

    private async Task UpdateRealmSettingsAsync()
    {
        var masterRealm = await _keycloakClient.GetRealmAsync(_keycloakOptions.RealmName);
        if (masterRealm.AccessTokenLifespan != 30 * 60)
        {
            masterRealm.AccessTokenLifespan = 30 * 60;
            await _keycloakClient.UpdateRealmAsync(_keycloakOptions.RealmName, masterRealm);
        }
    }

    private async Task CreateRoleMapperAsync()
    {
        var roleScope = (await _keycloakClient.GetClientScopesAsync(_keycloakOptions.RealmName))
            .FirstOrDefault(q => q.Name == "roles");
        if (roleScope == null)
            return;

        if (!roleScope.ProtocolMappers.Any(q => q.Name == "roles"))
        {
            await _keycloakClient.CreateProtocolMapperAsync(_keycloakOptions.RealmName, roleScope.Id,
                new ProtocolMapper()
                {
                    Name = "roles",
                    Protocol = "openid-connect",
                    _ProtocolMapper = "oidc-usermodel-realm-role-mapper",
                    Config = new Dictionary<string, string>()
                    {
                        { "access.token.claim", "true" },
                        { "id.token.claim", "true" },
                        { "claim.name", "role" },
                        { "multivalued", "true" },
                        { "userinfo.token.claim", "true" },
                    }
                });
        }
    }

    private async Task UpdateAdminUserAsync()
    {
        var users = await _keycloakClient.GetUsersAsync(_keycloakOptions.RealmName, username: "admin");
        var adminUser = users.FirstOrDefault();

        if (adminUser == null)
        {
            throw new Exception(
                "Keycloak admin user is not provided, check if KEYCLOAK_ADMIN environment variable is passed properly.");
        }

        var clientId = await GetClientIdAsync();
        var clientRoles = await _keycloakClient.GetRolesAsync(_keycloakOptions.RealmName, clientId);

        var userRoles = await _keycloakClient.GetRoleMappingsForUserAsync(_keycloakOptions.RealmName, adminUser.Id);

        var roles = userRoles.ClientMappings.FirstOrDefault(f => f.Key == _configuration[$"Clients:Web:Name"]);

        if (roles.Value == null || (roles.Value != null && !roles.Value.Mappings.Any(a => a.Name == "admin")))
        {
            var clientRole = clientRoles.FirstOrDefault(f => f.Name == "admin");

            var role = new List<Role> { new Role() { Id = clientRole.Id, Name = "admin", ClientRole = true, Composite = false } };

            await _keycloakClient.AddClientRoleMappingsToUserAsync(_keycloakOptions.RealmName, adminUser.Id, clientId, role);

            _logger.LogInformation("Updating admin user role...");
        }

        if (string.IsNullOrEmpty(adminUser.Email))
        {
            adminUser.Email = "admin@arslan.io";
            adminUser.FirstName = "admin";
            adminUser.EmailVerified = true;

            _logger.LogInformation("Updating admin user with email and first name...");
            await _keycloakClient.UpdateUserAsync(_keycloakOptions.RealmName, adminUser.Id, adminUser);
        }
    }
}
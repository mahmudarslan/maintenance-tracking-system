using Arslan.Vms.KeycloakService.Models;
using Flurl.Http;
using Keycloak.Net;
using Keycloak.Net.Models.Clients;
using Keycloak.Net.Models.ClientScopes;
using Keycloak.Net.Models.ProtocolMappers;
using Keycloak.Net.Models.Roles;
using Keycloak.Net.Models.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;

namespace Arslan.Vms.KeycloakService;

public class KeycloakServiceIntegration
{
    private readonly KeycloakClient _keycloakClient;
    private readonly KeycloakClientOptions _keycloakOptions;
    private readonly ILogger<KeycloakServiceIntegration> _logger;
    private readonly IConfiguration _configuration;

    public KeycloakServiceIntegration(IAbpApplicationWithInternalServiceProvider application)
    {
        _logger = application.ServiceProvider.GetRequiredService<ILogger<KeycloakServiceIntegration>>();
        _configuration = application.ServiceProvider.GetRequiredService<IConfiguration>();
        _keycloakOptions = new KeycloakClientOptions
        {
            Url = _configuration["Keycloak:url"],
            AdminUserName = _configuration["Keycloak:adminUsername"],
            AdminPassword = _configuration["Keycloak:adminPassword"],
            RealmName = _configuration["Keycloak:realmName"]
        };

        _keycloakClient = new KeycloakClient(_keycloakOptions.Url,
                                             _keycloakOptions.AdminUserName,
                                             _keycloakOptions.AdminPassword,
                                             new KeycloakOptions(adminClientId: "admin-cli",
                                                                 authenticationRealm: _configuration[$"Keycloak:realmName"]));
    }

    public async Task SeedDataAsync()
    {
        try
        {
            var tenants = _configuration.GetSection("Tenants").Get<List<TenantConfig>>() ?? new List<TenantConfig>();

            foreach (var tenant in tenants)
            {
                Log.Information("\n");
                Log.Information($"--------------[{tenant.Name}]------------------" + "\n");

                Log.Information($"[{tenant.Name}]->CreateRealm");

                await CreateRealmAsync(tenant);

                Log.Information($"[{tenant.Name}]->CreateRealmUser");
                await CreateRealmUserAsync(tenant);

                _keycloakOptions.RealmName = tenant.Name;

                foreach (var client in tenant.Clients)
                {
                    Log.Information($"[{tenant.Name}]->CreateClientScopes->{client.Name}");
                    await CreateClientScopesAsync(client.Scopes, tenant);

                    Log.Information($"[{tenant.Name}]->CreateClients->{client.Name}");

                    if (client.Type == "swagger")
                    {
                        await CreateSwaggerClientAsync(client, client.Scopes, tenant);
                    }
                    else
                    {
                        await CreateWebClientAsync(client, client.Scopes, tenant);

                        Log.Information($"[{tenant.Name}]->UpdateAdminUser->{client.Name}");
                        await UpdateAdminUserAsync(client);
                    }
                }
            }
            Log.Information("\n");
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        }
    }

    async Task CreateRealmAsync(TenantConfig tenant)
    {
        try
        {
            await _keycloakClient.ImportRealmAsync(_configuration[$"Keycloak:realmName"], new Keycloak.Net.Models.RealmsAdmin.Realm()
            {
                _Realm = tenant.Name,
                Enabled = true,
                LoginTheme = "default",
                ResetPasswordAllowed = true,
                EditUsernameAllowed = true,
                RegistrationEmailAsUsername = false,
                SsoSessionIdleTimeout = 604800,
                SsoSessionIdleTimeoutRememberMe = 5184000,
                SsoSessionMaxLifespan = 5184000,
                SsoSessionMaxLifespanRememberMe = 5184000,
                InternationalizationEnabled = true,
                SupportedLocales = new List<string>() { "en", "tr" },
                SmtpServer = new Keycloak.Net.Models.RealmsAdmin.SmtpServer()
                {
                    From = _configuration[$"RealmMail:From"],
                    FromDisplayName = _configuration[$"RealmMail:FromDisplayName"],
                    Host = _configuration[$"RealmMail:Host"]
                }
            });
        }
        catch (Exception ex)
        {
            Log.Error($"{_keycloakOptions.RealmName}]->CreateRealm->[Kayıtlı Tenant] " + "\n" + ex.Message + "\n");
        }
    }

    public async Task CreateRealmUserAsync(TenantConfig tenant)
    {
        try
        {
            var kUser = new User()
            {
                UserName = _configuration[$"Realm:Username"],
                FirstName = "",
                LastName = "",
                Email = _configuration[$"Realm:UserMail"],
                Enabled = true,
            };

            var keycloakUserId = await _keycloakClient.CreateAndRetrieveUserIdAsync(tenant.Name, kUser);

            await _keycloakClient.SetUserPasswordAsync(tenant.Name, keycloakUserId, _configuration[$"Realm:Password"]);

            kUser.Id = keycloakUserId;
        }
        catch (Exception ex)//Kullanıcı keycloak tarafında tanımılı ise update et
        {
            Log.Error($"{_keycloakOptions.RealmName}]->CreateRealmUser->[Kayıtlı Kullanıcı]" + "\n" + ex.Message + "\n");
        }
    }

    async Task CreateClientScopesAsync(List<ClientScopeModel> scopes, TenantConfig tenant = null)
    {
        foreach (var scope in scopes)
        {
            await CreateScopeAsync(scope.Name);
        }

        await CreateTenantScopeAsync(tenant);
    }

    async Task CreateScopeAsync(string scopeName)
    {
        try
        {
            var scope = (await _keycloakClient.GetClientScopesAsync(_keycloakOptions.RealmName))?.FirstOrDefault(q => q.Name == scopeName);

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
                                Dictionary<string,string>()
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
        catch (Exception ex)
        {
            Log.Error($"[{_keycloakOptions.RealmName}]->CreateScope->{scopeName}" + "\n" + ex.Message + "\n");
        }
    }

    async Task CreateTenantScopeAsync(TenantConfig tenant)
    {
        if (tenant == null)
        {
            return;
        }
        var tenantScopeName = "Tenant " + tenant.Name + " scope";
        try
        {
            var scope = (await _keycloakClient.GetClientScopesAsync(_keycloakOptions.RealmName))?.FirstOrDefault(q => q.Name == tenant.Name);

            if (scope == null)
            {
                scope = new ClientScope
                {
                    Name = tenant.Name,
                    Description = tenantScopeName,
                    Protocol = "openid-connect",
                    Attributes = new Attributes
                    {
                        ConsentScreenText = tenant.Name,
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
                                    Dictionary<string,string>()
                                    {
                                        { "access.token.claim", "true" },
                                        { "access.tokenResponse.claim", "false" },
                                        { "claim.name", "tenantid" },
                                        { "claim.value", tenant.Id },
                                        { "id.token.claim", "false" },
                                        { "userinfo.token.claim", "false" }
                                    }
                        }
                    }
                };

                await _keycloakClient.CreateClientScopeAsync(_keycloakOptions.RealmName, scope);
            }
        }
        catch (Exception ex)
        {
            Log.Error($"[{_keycloakOptions.RealmName}]->CreateTenantScope->{tenantScopeName}" + "\n" + ex.Message + "\n");
        }
    }

    async Task CreateWebClientAsync(ClientModel client, List<ClientScopeModel> scopes, TenantConfig tenant)
    {
        try
        {
            var webClient = (await _keycloakClient.GetClientsAsync(_keycloakOptions.RealmName, clientId: client.Name))?.FirstOrDefault();

            if (webClient == null)
            {
                var webRootUrl = client.RootUrl;
                var redirectUris = new List<string>();
                var scopeNames = new List<string>();

                foreach (var redirectUrl in client.RedirectUrls)
                {
                    redirectUris.Add(redirectUrl);
                }

                foreach (var scope in scopes)
                {
                    redirectUris.Add(scope.RootUrl.TrimEnd('/') + "/*");
                    scopeNames.Add(scope.Name);
                }

                webClient = new Client
                {
                    ClientId = client.Name,
                    Name = "Vms Web Application",
                    Protocol = "openid-connect",
                    Enabled = true,
                    BaseUrl = webRootUrl,
                    RedirectUris = redirectUris,
                    FrontChannelLogout = true,
                    PublicClient = true,
                    Attributes = new Dictionary<string, object>{
                                  { "post.logout.redirect.uris", $"{webRootUrl.TrimEnd('/')}/*" }},
                    WebOrigins = client.WebOrigins
                };

                foreach (var logoutRedirectUrl in client.LogoutRedirectUrls)
                {
                    try
                    {
                        webClient.Attributes.Add("post.logout.redirect.uris", logoutRedirectUrl);
                    }
                    catch (Exception)
                    {
                    }
                }

                await _keycloakClient.CreateClientAsync(_keycloakOptions.RealmName, webClient);

                await AddOptionalScopesAsync(
                                client.Name,
                                scopeNames);

                if (tenant != null)
                {
                    await AddDefaultClientScopesAsync(
                                    client.Name,
                                    new List<string>
                                    {
                                   tenant.Name
                                    });
                }

                foreach (var role in client.Roles)
                {
                    await CreateRoleAsync(client.Name, role.Name);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error($"[{_keycloakOptions.RealmName}]->CreateWebClient->{client.Name}" + "\n" + ex.Message + "\n");
        }
    }

    async Task CreateSwaggerClientAsync(ClientModel client, List<ClientScopeModel> scopes, TenantConfig tenant)
    {
        try
        {
            var swaggerClient = (await _keycloakClient.GetClientsAsync(_keycloakOptions.RealmName, clientId: client.Swagger.SwaggerClientId))?.FirstOrDefault();

            if (swaggerClient == null)
            {
                var redirectUris = new List<string>();
                var scopeNames = new List<string>();

                foreach (var redirectUrl in client.RedirectUrls)
                {
                    redirectUris.Add(redirectUrl);
                }

                foreach (var scope in scopes)
                {
                    redirectUris.Add(scope.RootUrl.TrimEnd('/') + "/swagger/oauth2-redirect.html");
                    scopeNames.Add(scope.Name);
                }

                swaggerClient = new Client
                {
                    ClientId = client.Swagger.SwaggerClientId,
                    Name = client.Swagger.Name,
                    Protocol = "openid-connect",
                    Enabled = true,
                    RedirectUris = redirectUris,
                    FrontChannelLogout = true,
                    PublicClient = false,
                    ImplicitFlowEnabled = false,
                    DirectAccessGrantsEnabled = false,
                    StandardFlowEnabled = true,
                    ServiceAccountsEnabled = false,
                    SurrogateAuthRequired = false,
                    Secret = client.Swagger.SwaggerClientSecret,
                    BearerOnly = false,
                    ClientAuthenticatorType = "client-secret",
                    Attributes = new Dictionary<string, object>(),
                    WebOrigins = client.WebOrigins
                };

                foreach (var logoutRedirectUrl in client.LogoutRedirectUrls)
                {
                    try
                    {
                        swaggerClient.Attributes.Add("post.logout.redirect.uris", logoutRedirectUrl);
                    }
                    catch (Exception)
                    {
                    }
                }

                await _keycloakClient.CreateClientAsync(_keycloakOptions.RealmName, swaggerClient);

                await AddOptionalScopesAsync(
                                client.Swagger.SwaggerClientId,
                                scopeNames);

                if (tenant != null)
                {
                    await AddDefaultClientScopesAsync(
                                    client.Swagger.SwaggerClientId,
                                    new List<string>
                                    {
                                   tenant.Name
                                    });
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error($"[{_keycloakOptions.RealmName}]->CreateSwaggerClient->{client.Name}" + "\n" + ex.Message + "\n");
        }
    }

    async Task AddDefaultClientScopesAsync(string clientName, List<string> scopes)
    {
        try
        {
            var client = (await _keycloakClient.GetClientsAsync(_keycloakOptions.RealmName, clientId: clientName))?.FirstOrDefault();

            if (client == null)
            {
                _logger.LogError($"[{_keycloakOptions.RealmName}]->AddDefaultClientScopes->Couldn't find {clientName}! Could not seed optional scopes!");
                return;
            }

            var clientOptionalScopes =
                (await _keycloakClient.GetDefaultClientScopesAsync(_keycloakOptions.RealmName, client.Id)).ToList();

            var clientScopes = (await _keycloakClient.GetClientScopesAsync(_keycloakOptions.RealmName)).ToList();

            foreach (var scope in scopes)
            {
                if (!clientOptionalScopes.Any(q => q.Name == scope))
                {
                    try
                    {
                        var serviceScope = clientScopes.First(q => q.Name == scope);

                        _logger.LogInformation($"[{_keycloakOptions.RealmName}]->AddOptionalScopes->{scope} {clientName}");

                        await _keycloakClient.UpdateDefaultClientScopeAsync(_keycloakOptions.RealmName,
                                                                            client.Id,
                                                                            serviceScope.Id);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"[{_keycloakOptions.RealmName}]->AddDefaultClientScopes->{clientName}->{scope}" + "\n" + ex.Message + "\n");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error($"[{_keycloakOptions.RealmName}]->AddDefaultClientScopes->{clientName}" + "\n" + ex.Message + "\n");
        }
    }

    async Task AddOptionalScopesAsync(string clientName, List<string> scopes)
    {
        try
        {
            var client = (await _keycloakClient.GetClientsAsync(_keycloakOptions.RealmName, clientId: clientName))?.FirstOrDefault();

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
                    try
                    {
                        var serviceScope = clientScopes.First(q => q.Name == scope);

                        _logger.LogInformation($"[{_keycloakOptions.RealmName}]->AddOptionalScopes->{scope} {clientName}");

                        await _keycloakClient.UpdateOptionalClientScopeAsync(_keycloakOptions.RealmName,
                                                                             client.Id,
                                                                             serviceScope.Id);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"[{_keycloakOptions.RealmName}]->AddDefaultClientScopes->{clientName}->{scope}" + "\n" + ex.Message + "\n");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error($"[{_keycloakOptions.RealmName}]->AddOptionalScopes->{clientName}" + "\n" + ex.Message + "\n");
        }
    }

    public async Task CreateRoleAsync(string clientName, string roleName)
    {
        var clientId = await GetKeycloakClientIdAsync(clientName);

        try
        {
            var keycloakRole = await _keycloakClient.GetRoleByNameAsync(_keycloakOptions.RealmName, clientId, roleName);
        }
        catch (FlurlHttpException ex)
        {
            //if (ex.StatusCode == 404)
            {
                var role = new Role()
                {
                    Name = roleName,
                };
                await _keycloakClient.CreateRoleAsync(_keycloakOptions.RealmName, clientId, role);
            }
        }
    }

    public async Task<string> GetKeycloakClientIdAsync(string clientName)
    {
        try
        {
            var clients = await _keycloakClient.GetClientsAsync(_keycloakOptions.RealmName, clientName);
            var _client = clients.FirstOrDefault(f => f.ClientId == clientName);

            return _client.Id;
        }
        catch (Exception ex)
        {
            Log.Error($"[{_keycloakOptions.RealmName}]->GetKeycloakClientId->{clientName}" + "\n" + ex.Message + "\n");
        }

        return null;
    }

    async Task UpdateAdminUserAsync(ClientModel client)
    {
        try
        {
            var users = await _keycloakClient.GetUsersAsync(_keycloakOptions.RealmName, username: "admin");
            var adminUser = users.FirstOrDefault();
            //var web = clients.FirstOrDefault(f => f.Type == "web");

            if (adminUser == null)
            {
                Log.Error($"[{_keycloakOptions.RealmName}]->UpdateAdminUser->Keycloak admin user is not provided, check if KEYCLOAK_ADMIN environment variable is passed properly.");
                return;
            }

            var clientId = await GetKeycloakClientIdAsync(client.Name);
            var clientRoles = await _keycloakClient.GetRolesAsync(_keycloakOptions.RealmName, clientId);

            var userRoles = await _keycloakClient.GetRoleMappingsForUserAsync(_keycloakOptions.RealmName, adminUser.Id);

            var roles = userRoles?.ClientMappings?.FirstOrDefault(f => f.Key == client.Name);

            if (roles == null || roles.Value.Key == null || roles != null && roles.Value.Value != null && !roles.Value.Value.Mappings.Any(a => a.Name == "admin"))
            {
                var clientRole = clientRoles.FirstOrDefault(f => f.Name == "admin");

                var role = new List<Role> { new Role() { Id = clientRole.Id, Name = "admin", ClientRole = true, Composite = false } };

                await _keycloakClient.AddClientRoleMappingsToUserAsync(_keycloakOptions.RealmName, adminUser.Id, clientId, role);

                _logger.LogInformation($"[{_keycloakOptions.RealmName}]->UpdateAdminUser->Updating admin user role...");
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
        catch (Exception ex)
        {
            Log.Error($"[{_keycloakOptions.RealmName}]->UpdateAdminUser->{client.Name}" + "\n" + ex.Message + "\n");
        }
    }
}

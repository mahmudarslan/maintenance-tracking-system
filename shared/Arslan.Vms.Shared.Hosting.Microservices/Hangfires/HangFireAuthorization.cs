using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Arslan.Vms.Shared.Hosting.Microservices.Hangfires;

public class CustomHangFireMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public CustomHangFireMiddleware(RequestDelegate next,
                                    IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var host = httpContext.Request.Headers["X-Forwarded-Host"].ToString();
        var proto = httpContext.Request.Headers["X-Forwarded-Proto"].ToString();

        if (!string.IsNullOrEmpty(proto))
        {
            proto = proto.Contains("https") ? "https" : "http";
        }
        else
        {
            proto = httpContext.Request.Scheme;
        }

        if (string.IsNullOrEmpty(host))
        {
            host = httpContext.Request.Host.ToString();
        }

        var tenant = host.Split('.').First();

        if (tenant == null || tenant.Contains("localhost"))
        {
            tenant = _configuration["Tenants:DefaultTenant"];
        }

        var code = httpContext.Request.Query.FirstOrDefault(a => a.Key == "code" && !string.IsNullOrEmpty(a.Key));
        var token = httpContext.Request.Cookies["HangFireCookie"];
        var isRole = false;
        var keycloakUrl = _configuration["AuthServer:Authority"] + "/" + tenant;
        var clientId = _configuration["AuthServer:Audience"];
        var apiUrl = $"{proto}://{host}";//_configuration["App:SelfUrl"];
        var pathBase = _configuration["App:PathBase"].Trim('/');
        var pathMatch = _configuration["Hangfire:Endpoint"].Trim('/');
        var role = "admin";

        pathMatch = string.IsNullOrEmpty(pathBase) ? $"{pathMatch}" : $"{pathBase}/{pathMatch}";

        var url = $"{apiUrl}/{pathMatch}";

        if (code.Key == null && token == null)
        {
            httpContext.Response.StatusCode = 301;
            httpContext.Response.Redirect($"{keycloakUrl}/protocol/openid-connect/auth?client_id={clientId}&redirect_uri={url}&response_mode=query&response_type=code&scope=roles");
        }
        else if (token == null)
        {
            try
            {
                var result = await keycloakUrl
                                          .AppendPathSegment($"/protocol/openid-connect/token")
                                          .WithHeader("Content-Type", "application/x-www-form-urlencoded")
                                          .PostUrlEncodedAsync(new List<KeyValuePair<string, string>>
                                          {
                                               new KeyValuePair<string, string>("grant_type", "authorization_code"),
                                               new KeyValuePair<string, string>("code", code.Value),
                                               new KeyValuePair<string, string>("client_id", clientId),
                                               new KeyValuePair<string, string>("redirect_uri", $"{url}")
                                          })
                                          .ReceiveJson().ConfigureAwait(false);

                token = result.access_token.ToString();

                httpContext.Response.Cookies.Append("HangFireCookie",
                                                     token,
                                                     new CookieOptions()
                                                     {
                                                         Expires = DateTime.Now.AddMinutes(10),
                                                         Path = $"/{pathMatch}"
                                                     });

                isRole = IsInRole(token, clientId, role);
            }
            catch (Exception)
            {
                httpContext.Response.StatusCode = 301;
                httpContext.Response.Redirect($"{url}");
            }
        }
        else
        {
            isRole = IsInRole(token, clientId, role);
        }

        if (isRole)
        {
            await _next.Invoke(httpContext);
        }
        else
        {
            await httpContext.Response.WriteAsync("Yetkiniz yok!!!");
        }
    }

    static bool IsInRole(string token, string clientId, string role)
    {
        string[] split = token.Split('.');
        var jsonHeaderData = JsonConvert.DeserializeObject<JwtHeader>(Base64DecodeToString(split[0]));

        string jsonData = Base64DecodeToString(split[1]);

        string verification = split[2];

        var roles = JObject.Parse(jsonData)["resource_access"][clientId]["roles"].Values<string>().ToList();

        return roles.Any(a => a == role);
    }

    static string Base64DecodeToString(string ToDecode)
    {
        string decodePrepped = ToDecode.Replace("-", "+").Replace("_", "/");

        switch (decodePrepped.Length % 4)
        {
            case 0:
                break;
            case 2:
                decodePrepped += "==";
                break;
            case 3:
                decodePrepped += "=";
                break;
            default:
                throw new Exception("Not a legal base64 string!");
        }

        byte[] data = Convert.FromBase64String(decodePrepped);
        return System.Text.Encoding.UTF8.GetString(data);
    }
}
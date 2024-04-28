using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Arslan.Vms.Shared.Hosting.Microservices;

public class CustomResponseHeader1AddMiddleware
{
    private readonly RequestDelegate _next;

    public CustomResponseHeader1AddMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting((state) =>
        {
            if (context.Response.StatusCode == (int)HttpStatusCode.OK)
            {
                try
                {
                    context.Response.Headers.Add($"MachineName-1", $"{Environment.MachineName}");
                }
                catch (Exception ex)
                {
                    Log.Error("context.Response.Headers.Add(MachineName)->" + ex.Message);
                }
            }
            return Task.FromResult(0);
        }, null);

        await _next(context);
    }
}

public class CustomResponseHeader2AddMiddleware
{
    private readonly RequestDelegate _next;

    public CustomResponseHeader2AddMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting((state) =>
        {
            if (context.Response.StatusCode == (int)HttpStatusCode.OK)
            {
                try
                {
                    context.Response.Headers.Add($"MachineName-2", $"{Environment.MachineName}");
                }
                catch (Exception ex)
                {
                    Log.Error("context.Response.Headers.Add(MachineName)->" + ex.Message);
                }
            }
            return Task.FromResult(0);
        }, null);

        await _next(context);
    }
}


public static class CustomResponseHeaderAddExtensions
{
    public static IApplicationBuilder UseAddMachineName1ResponseHeader(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomResponseHeader1AddMiddleware>();

        return app;
    }

    public static IApplicationBuilder UseAddMachineName2ResponseHeader(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomResponseHeader2AddMiddleware>();

        return app;
    }
}
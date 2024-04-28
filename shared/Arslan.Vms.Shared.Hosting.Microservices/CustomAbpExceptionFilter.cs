using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Json;

namespace Arslan.Vms.Shared.Hosting.Microservices;

public class CustomAbpExceptionFilter : IAsyncExceptionFilter, IFilterMetadata, ITransientDependency
{
    public virtual async Task OnExceptionAsync(ExceptionContext context)
    {
        if (!ShouldHandleException(context))
        {
            LogException(context, out var _);
        }
        else
        {
            await HandleAndWrapException(context).ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    protected virtual bool ShouldHandleException(ExceptionContext context)
    {
        if (context.ActionDescriptor.IsControllerAction() && context.ActionDescriptor.HasObjectResult())
        {
            return true;
        }
        if (context.HttpContext.Request.CanAccept("application/json"))
        {
            return true;
        }
        if (context.HttpContext.Request.IsAjax())
        {
            return true;
        }
        return false;
    }

    protected virtual async Task HandleAndWrapException(ExceptionContext context)
    {
        LogException(context, out var remoteServiceErrorInfo);
        await context.GetRequiredService<IExceptionNotifier>().NotifyAsync(new ExceptionNotificationContext(context.Exception)).ConfigureAwait(continueOnCapturedContext: false);
        if (context.Exception is AbpAuthorizationException)
        {
            await context.HttpContext.RequestServices.GetRequiredService<IAbpAuthorizationExceptionHandler>().HandleAsync(context.Exception.As<AbpAuthorizationException>(), context.HttpContext).ConfigureAwait(continueOnCapturedContext: false);
        }
        else
        {
            context.HttpContext.Response.Headers.Add("_AbpErrorFormat", "true");
            context.HttpContext.Response.StatusCode = (int)context.GetRequiredService<IHttpExceptionStatusCodeFinder>().GetStatusCode(context.HttpContext, context.Exception);
            context.Result = new ObjectResult(new RemoteServiceErrorResponse(remoteServiceErrorInfo));
        }
        context.Exception = null;
    }

    protected virtual void LogException(ExceptionContext context, out RemoteServiceErrorInfo remoteServiceErrorInfo)
    {
        AbpExceptionHandlingOptions exceptionHandlingOptions = context.GetRequiredService<IOptions<AbpExceptionHandlingOptions>>().Value;
        IExceptionToErrorInfoConverter requiredService = context.GetRequiredService<IExceptionToErrorInfoConverter>();
        remoteServiceErrorInfo = requiredService.Convert(context.Exception, delegate (AbpExceptionHandlingOptions options)
        {
            options.SendExceptionsDetailsToClients = exceptionHandlingOptions.SendExceptionsDetailsToClients;
            options.SendStackTraceToClients = exceptionHandlingOptions.SendStackTraceToClients;
        });
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("---------- RemoteServiceErrorInfo ----------");
        stringBuilder.AppendLine(context.GetRequiredService<IJsonSerializer>().Serialize(remoteServiceErrorInfo, camelCase: true, indented: true));
        ILogger<AbpExceptionFilter> service = context.GetService((ILogger<AbpExceptionFilter>)NullLogger<AbpExceptionFilter>.Instance);
        LogLevel logLevel = context.Exception.GetLogLevel();
        //service.LogWithLevel(logLevel, stringBuilder.ToString());
        //service.LogException(context.Exception, logLevel);

        service.LogWithLevel(logLevel, context.Exception.ToString() + "\n\n" + stringBuilder.ToString());
    }
}

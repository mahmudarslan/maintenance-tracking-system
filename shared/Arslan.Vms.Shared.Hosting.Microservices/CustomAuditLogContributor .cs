using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Data;

namespace Arslan.Vms.Shared.Hosting.Microservices;

public class CustomAuditLogContributor : AuditLogContributor
{
    public override void PreContribute(AuditLogContributionContext context)
    {
        //var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
        //context.AuditInfo.SetProperty(
        //	"MyCustomClaimValue",
        //	currentUser.FindClaimValue("MyCustomClaim")
        //);
    }

    public override void PostContribute(AuditLogContributionContext context)
    {
        var request = context.GetHttpContext().Request;
        //context.AuditInfo.Comments.Add($"{request.Scheme}://{request.Host}/{request.PathBase}");

        context.AuditInfo.SetProperty(
             "Host",
            $"{request.Scheme}://{request.Host}{request.PathBase}"
        );

        if (!string.IsNullOrEmpty(request.Headers["X-Forwarded-Proto"]))
        {
            context.AuditInfo.SetProperty(
                    "X-Forwarded-Proto",
                   $"{request.Headers["X-Forwarded-Proto"]}"
                );
        }

        if (!string.IsNullOrEmpty(request.Headers["X-Forwarded-Host"]))
        {
            context.AuditInfo.SetProperty(
                    "X-Forwarded-Host",
                   $"{request.Headers["X-Forwarded-Host"]}"
                );
        }
    }
}
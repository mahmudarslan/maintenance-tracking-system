using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Arslan.Vms.Shared.Hosting.Microservices.Auths;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDynamicAuthorityJwtBearerResolver<TAuthoritySolver>(this IServiceCollection services)
        where TAuthoritySolver : class, IDynamicJwtBearerAuthorityResolver
    {
        services.AddTransient<IDynamicJwtBearerAuthorityResolver, TAuthoritySolver>();
        services.AddSingleton<IDynamicJwtBearerHanderConfigurationResolver, DynamicAuthorityJwtBearerHandlerConfigurationResolver>();
        return services;
    }

    public static AuthenticationBuilder AddDynamicAuthorityJwtBearerResolver<TAuthoritySolver>(this AuthenticationBuilder authenticationBuilder)
        where TAuthoritySolver : class, IDynamicJwtBearerAuthorityResolver
    {
        authenticationBuilder.Services.AddDynamicAuthorityJwtBearerResolver<TAuthoritySolver>();
        return authenticationBuilder;
    }
}
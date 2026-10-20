using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NET.Backend.Blueprint.Web.Authorization;
using NET.Backend.Blueprint.Web.ErrorHandling;
using NET.Backend.Blueprint.Web.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NET.Backend.Blueprint.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlueprintSecurity(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters.NameClaimType = KeycloakClaims.UserName;
                options.TokenValidationParameters.RoleClaimType = KeycloakClaims.Roles;
            });
        services.AddSingleton<IAuthorizationHandler, ApiAccessHandler>();
        services.AddAuthorizationBuilder()
            .AddPolicy(Policies.ApiAccess, policy => policy
                .RequireAuthenticatedUser()
                .AddRequirements(new ApiAccessRequirement()));
        return services;
    }

    public static IServiceCollection AddBlueprintProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<BlueprintExceptionHandler>();
        return services;
    }

    public static IServiceCollection AddVersionedSwagger(this IServiceCollection services, string title)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSingleton<IConfigureOptions<SwaggerGenOptions>>(provider =>
            new ConfigureSwaggerOptions(provider.GetRequiredService<IApiVersionDescriptionProvider>(), title));
        return services;
    }
}

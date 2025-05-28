using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.RateLimiting;

namespace NET.Backend.Blueprint.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddRateLimiterBlueprint(this IServiceCollection source)
    {
        source.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("fixed", opt =>
            {
                opt.PermitLimit = 1;
                opt.Window = TimeSpan.FromSeconds(2);
                opt.QueueLimit = 0;
            });
            options.RejectionStatusCode = StatusCodes.Status400BadRequest;
        });
    }
    public static void AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ReportApiVersions = true;
                o.ApiVersionReader = new HeaderApiVersionReader();
            }).AddMvc(o =>
            {
                o.Conventions.Add(new VersionByNamespaceConvention());
            })
            .AddApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'V";
                o.SubstituteApiVersionInUrl = true;
            });
    }
}
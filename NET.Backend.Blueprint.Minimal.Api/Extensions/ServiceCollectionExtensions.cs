using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using NET.Backend.Blueprint.Minimal.Api.RateLimiting;

namespace NET.Backend.Blueprint.Minimal.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlueprintJson(this IServiceCollection services)
    {
        return services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    }

    public static IServiceCollection AddFixedWindowRateLimiting(this IServiceCollection services)
    {
        services.AddOptions<FixedWindowRateLimitSettings>()
            .BindConfiguration(FixedWindowRateLimitSettings.SectionName);

        services.AddRateLimiter(options => options.RejectionStatusCode = StatusCodes.Status429TooManyRequests);
        services.AddOptions<RateLimiterOptions>()
            .Configure<IOptions<FixedWindowRateLimitSettings>>((options, settings) =>
                options.AddFixedWindowLimiter(FixedWindowRateLimitSettings.PolicyName, limiter =>
                {
                    limiter.PermitLimit = settings.Value.PermitLimit;
                    limiter.Window = settings.Value.Window;
                    limiter.QueueLimit = settings.Value.QueueLimit;
                    limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                }));

        return services;
    }

    public static IServiceCollection AddBlueprintVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }
}

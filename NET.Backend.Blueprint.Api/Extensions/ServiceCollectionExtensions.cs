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
}
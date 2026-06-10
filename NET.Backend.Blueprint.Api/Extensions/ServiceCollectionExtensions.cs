using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.RateLimiting;
using NET.Backend.Blueprint.Api.CQRS.Commands;
using System.Reflection;

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
            }).AddMvc(o => { o.Conventions.Add(new VersionByNamespaceConvention()); })
            .AddApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'V";
                o.SubstituteApiVersionInUrl = true;
            });
    }


    public static void AddCqrs(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var types = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract);

        foreach (var type in types)
        {
            foreach (var i in type.GetInterfaces())
            {
                if (!i.IsGenericType)
                    continue;

                var generic = i.GetGenericTypeDefinition();

                if (generic == typeof(ICommandHandler<>) ||
                    generic == typeof(ICommandHandler<,>) ||
                    generic == typeof(IQueryHandler<,>))
                {
                    services.AddScoped(i, type);
                }
            }
        }
    }
}
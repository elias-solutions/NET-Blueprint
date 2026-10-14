using Microsoft.Extensions.DependencyInjection;
using NET.Backend.Blueprint.Application.CQRS;

namespace NET.Backend.Blueprint.Application.Extensions;

public static class ServiceCollectionExtensions
{
    private static readonly Type[] HandlerTypes =
    [
        typeof(ICommandHandler<>),
        typeof(ICommandHandler<,>),
        typeof(IQueryHandler<,>)
    ];

    public static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        var implementations = typeof(ServiceCollectionExtensions).Assembly.GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false });

        foreach (var implementation in implementations)
        {
            var handlerInterfaces = implementation.GetInterfaces()
                .Where(i => i.IsGenericType && HandlerTypes.Contains(i.GetGenericTypeDefinition()));

            foreach (var handlerInterface in handlerInterfaces)
            {
                services.AddScoped(handlerInterface, implementation);
            }
        }

        return services;
    }
}

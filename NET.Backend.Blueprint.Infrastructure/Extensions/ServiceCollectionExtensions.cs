using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NET.Backend.Blueprint.Application.Persistence;
using NET.Backend.Blueprint.Infrastructure.DataAccess;

namespace NET.Backend.Blueprint.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, Action<IServiceProvider, DbContextOptionsBuilder> configureDatabase)
    {
        services.AddDbContextFactory<BlueprintDbContext>(configureDatabase);
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }
}

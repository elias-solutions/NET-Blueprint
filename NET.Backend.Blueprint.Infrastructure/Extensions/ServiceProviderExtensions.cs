using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NET.Backend.Blueprint.Infrastructure.DataAccess;

namespace NET.Backend.Blueprint.Infrastructure.Extensions;

public static class ServiceProviderExtensions
{
    public static async Task MigrateDatabaseAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        var contextFactory = services.GetRequiredService<IDbContextFactory<BlueprintDbContext>>();
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        await context.Database.MigrateAsync(cancellationToken);
    }
}

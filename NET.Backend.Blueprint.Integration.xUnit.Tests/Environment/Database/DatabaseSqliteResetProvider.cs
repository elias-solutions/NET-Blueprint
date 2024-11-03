using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NET.Backend.Blueprint.Api.DataAccess;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment.Database;

public class DatabaseSqliteResetProvider(IServiceProvider serviceProvider) : IDatabaseResetProvider
{
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task ResetAsync()
    {
        var scopeFactory = serviceProvider.GetService<IServiceScopeFactory>()!;
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetService<BlueprintDbContext>()!;
        await context.Database.EnsureCreatedAsync();

        context.Persons.RemoveRange(context.Persons);
        await context.SaveChangesAsync();
    }

    public Task DisposeDbConnectionAsync() => Task.CompletedTask;
}
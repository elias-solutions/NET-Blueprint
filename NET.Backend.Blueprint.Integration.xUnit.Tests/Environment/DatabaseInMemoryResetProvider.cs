using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.DataAccess;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment.Database;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;

public class DatabaseInMemoryResetProvider(IDbContextFactory<BlueprintDbContext> contextFactory) : IDatabaseResetProvider
{
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task ResetAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var persons = await context.Persons.ToListAsync();
        var addresses = await context.Addresses.ToListAsync();

        context.Addresses.RemoveRange(addresses);
        context.Persons.RemoveRange(persons);

        await context.SaveChangesAsync();
    }

    public Task DisposeDbConnectionAsync() => Task.CompletedTask;
}
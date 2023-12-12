using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Adapter;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.SQlite.Environment;

public class DatabaseResetProvider
{
    public DatabaseResetProvider()
    {
        var scopeFactory = _fixture.Server.Services.GetService<IServiceScopeFactory>()!;
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetService<Blueprint>()!;
        context.Addresses.ToList().ForEach(address =>
        {
            address.City = string.Empty;
            address.Country = string.Empty;
            address.FsoCode = string.Empty;
            address.Canton = string.Empty;
        });
        await context.SaveChangesAsync();
    }


    public async Task InitializeAsync()
    {
    }
    
    public async Task ResetAsync() => await _respawner.ResetAsync(_dbConnection);
    
    public async Task DisposeDbConnectionAsync() => await _dbConnection.DisposeAsync();
}
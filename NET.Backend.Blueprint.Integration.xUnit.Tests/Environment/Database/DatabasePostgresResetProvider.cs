using System.Data.Common;
using Microsoft.Data.SqlClient;
using Respawn;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment.Database;

public class DatabasePostgresResetProvider : IDatabaseResetProvider
{
    private readonly DbConnection _dbConnection = 
        new SqlConnection("Server=localhost;Database=BlueprintDatabaseTest;User AddressId=dev;Password=dev;");
    private Respawner respawner = default!;

    public async Task InitializeAsync()
    {
        var options = new RespawnerOptions { DbAdapter = DbAdapter.SqlServer };
        await _dbConnection.OpenAsync();
        respawner = await Respawner.CreateAsync(_dbConnection, options);
    }

    public async Task ResetAsync() => await respawner.ResetAsync(_dbConnection);

    public async Task DisposeDbConnectionAsync() => await _dbConnection.DisposeAsync();
}
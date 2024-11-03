using System.Data.Common;
using Microsoft.Data.SqlClient;
using Respawn;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment.Database;

public class DatabaseMssqlResetProvider : IDatabaseResetProvider
{
    private readonly DbConnection _dbConnection = new SqlConnection("Server=localhost;Database=BlueprintDatabaseTest;TrustServerCertificate=True;Integrated Security=True;");
    private Respawner respawner = default!;

    public async Task InitializeAsync()
    {
        var options = new RespawnerOptions { DbAdapter = DbAdapter.SqlServer };
        await _dbConnection.OpenAsync();
        respawner = await Respawner.CreateAsync(_dbConnection, options);
    }

    public async Task ResetAsync()
    {
        await respawner.ResetAsync(_dbConnection);
    }

    public async Task DisposeDbConnectionAsync() => await _dbConnection.DisposeAsync();
}
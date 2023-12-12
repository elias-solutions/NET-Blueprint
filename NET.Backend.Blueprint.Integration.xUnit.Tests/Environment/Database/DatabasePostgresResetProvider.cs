using Npgsql;
using Respawn;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment.Database;

public class DatabasePostgresResetProvider : IDatabaseResetProvider
{
    private readonly NpgsqlConnection _dbConnection = new("Host=localhost; Database=BlueprintDatabaseTest; Username=dev; Password=dev");
    private Respawner _respawner = default!;

    public async Task InitializeAsync()
    {
        var options = new RespawnerOptions
        {
            SchemasToInclude = new[] { "public" },
            DbAdapter = DbAdapter.Postgres
        };

        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, options);
    }

    public async Task ResetAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public async Task DisposeDbConnectionAsync() => await _dbConnection.DisposeAsync();
}
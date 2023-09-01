using Npgsql;
using Respawn;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;

public class PostgresDbResetProvider
{
    private readonly NpgsqlConnection _dbConnection;
    private Respawner _respawner = default!;

    public PostgresDbResetProvider(string connectionString)
    {
        _dbConnection = new(connectionString);
    }
    
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
    
    public async Task ResetAsync() => await _respawner.ResetAsync(_dbConnection);
    
    public async Task DisposeDbConnectionAsync() => await _dbConnection.DisposeAsync();
}
using Npgsql;
using Respawn;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;

public class RespawnerHelper
{
    private readonly NpgsqlConnection _dbConnection;
    private Respawner _respawner = default!;

    public RespawnerHelper(string connectionString)
    {
        _dbConnection = new(connectionString);
    }
    
    public async Task InitializeRespawner()
    {
        var options = new RespawnerOptions
        {
            SchemasToInclude = new[] { "public" },
            DbAdapter = DbAdapter.Postgres
        };
        
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, options);
    }
    
    public async Task ResetRespawner() => await _respawner.ResetAsync(_dbConnection);
    
    public async Task CloseDbConnectionRespawner() => await _dbConnection.DisposeAsync();
}
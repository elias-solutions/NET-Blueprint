using BIT.NET.Backend.Blueprint.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using NSubstitute;
using Respawn;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;

public abstract class TestBase : IClassFixture<WebApplicationFactory<Startup>>, IAsyncLifetime
{
    private Respawner _respawner = default!;
    private readonly NpgsqlConnection _dbConnection = new("Host=localhost; Database=BlueprintDatabase; Username=dev; Password=dev");
    private WebApplicationFactory<Startup> Factory { get; }
    protected IUserService UserService { get; }
    protected HttpClient Client { get; }


    protected TestBase(WebApplicationFactory<Startup> factory)
    {
        UserService = Substitute.For<IUserService>();

        Factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                ServiceCollectionServiceExtensions.AddScoped(services, _ => UserService);
                AuthenticationServiceCollectionExtensions.AddAuthentication(services, "TestAuthentication")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("TestAuthentication", null);
            });
        });

        Client = Factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        await InitializeRespawner();
        await InitAsync();
    }

    private async Task InitializeRespawner()
    {
        var options = new RespawnerOptions
        {
            SchemasToInclude = new[] { "public" },
            DbAdapter = DbAdapter.Postgres
        };
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, options);
    }
    
    public async Task DisposeAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
        await DeInitAsync();
    } 

    protected abstract Task InitAsync();
    protected abstract Task DeInitAsync();
}
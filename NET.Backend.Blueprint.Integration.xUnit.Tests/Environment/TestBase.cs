using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NET.Backend.Blueprint.Api;
using NET.Backend.Blueprint.Api.Authorization;
using NET.Backend.Blueprint.Api.DataAccess;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment.Database;
using NSubstitute;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;

public abstract class TestBase : WebApplicationFactory<Startup>, IAsyncLifetime
{
    private SqliteConnection _connection = default!;
    public IUserService UserService { get; }
    public IDatabaseResetProvider DatabaseResetProvider { get; }
    protected HttpClient Client { get; }
    
    protected TestBase()
    {
        UserService = Substitute.For<IUserService>();
        Client = CreateClient();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        DatabaseResetProvider = new DatabaseSqliteResetProvider(Services);
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {            
        builder
            .ConfigureTestServices(services =>
            {
                RemoveDbContext(services);
                services.AddDbContextFactory<BlueprintDbContext>(options =>
                {
                    _connection = new SqliteConnection("Data Source=:memory:");
                    _connection.Open();

                    var optionsBuilder = options.UseSqlite(_connection);
                    var context = new BlueprintDbContext(optionsBuilder.Options);
                    context.Database.EnsureCreated();
                });

                services.AddSingleton(_ => DatabaseResetProvider);
                services.AddScoped(_ => UserService);
                services.AddAuthentication("TestAuthentication")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("TestAuthentication", null);
            });
    }

    public async Task InitializeAsync() => await DatabaseResetProvider.InitializeAsync();

    async Task IAsyncLifetime.DisposeAsync()
    {
        await DatabaseResetProvider.DisposeDbConnectionAsync();
        await _connection.CloseAsync();
    }

    private static void RemoveDbContext(IServiceCollection services)
    {
        var dbContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BlueprintDbContext>))!;
        services.Remove(dbContext);
    }
}
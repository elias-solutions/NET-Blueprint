using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    private IConfiguration _configuration = default!;
    public IUserService UserService { get; }
    public IDatabaseResetProvider DatabaseResetProvider { get; }
    protected HttpClient Client { get; }
    
    protected TestBase()
    {
        UserService = Substitute.For<IUserService>();
        Client = CreateClient();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        DatabaseResetProvider = new DatabaseMssqlResetProvider();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Add configuration sources
            config.AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            _configuration = config.Build();
        });

        builder
            .ConfigureTestServices(services =>
            {
                RemoveDbContext(services);
                services.AddDbContextFactory<BlueprintDbContext>(options =>
                {
                    options.UseSqlServer(_configuration.GetConnectionString("DatabaseTest"));

                    var context = new BlueprintDbContext(options.Options);
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
    }

    private static void RemoveDbContext(IServiceCollection services)
    {
        var dbContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BlueprintDbContext>))!;
        services.Remove(dbContext);
    }
}
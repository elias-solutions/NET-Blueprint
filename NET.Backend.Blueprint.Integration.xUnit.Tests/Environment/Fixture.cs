using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NET.Backend.Blueprint.Web.Authorization;
using NET.Backend.Blueprint.Infrastructure.DataAccess;
using NSubstitute;
using Testcontainers.PostgreSql;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;

public class Fixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _database = new PostgreSqlBuilder("postgres:18-alpine").Build();

    public IUserService UserService { get; } = Substitute.For<IUserService>();
    private HttpClient Client { get; set; } = null!;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Add configuration sources
            config.AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:Database"] = _database.GetConnectionString()
                });
        });

        builder
            .ConfigureTestServices(services =>
            {
                services.AddScoped(_ => UserService);
                services.AddAuthentication("TestAuthentication")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("TestAuthentication", null);
            });
    }

    public async Task InitializeAsync()
    {
        await _database.StartAsync();

        Client = CreateClient();
        Client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        Client?.Dispose();
        await _database.DisposeAsync();
    }

    public async Task<HttpResponseMessage> SendAsync(HttpMethod method, string route, HttpContent? content, User? user)
    {
        UserService.GetCurrentUser().Returns(user);

        var message = new HttpRequestMessage
        {
            Method = method,
            RequestUri = new Uri(route, UriKind.Relative),
            Content = content
        };

        return await Client.SendAsync(message);
    }

    public async Task ResetDatabaseAsync()
    {
        var contextFactory = Services.GetRequiredService<IDbContextFactory<BlueprintDbContext>>();
        await using var dbContext = await contextFactory.CreateDbContextAsync();

        dbContext.Addresses.RemoveRange(dbContext.Addresses);
        dbContext.Persons.RemoveRange(dbContext.Persons);
        await dbContext.SaveChangesAsync();
    }
}
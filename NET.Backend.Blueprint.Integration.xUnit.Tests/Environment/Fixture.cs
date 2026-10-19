using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NET.Backend.Blueprint.Infrastructure.DataAccess;
using Testcontainers.PostgreSql;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;

public class Fixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _database = new PostgreSqlBuilder("postgres:18-alpine").Build();

    public TestTokenIssuer Tokens { get; } = new();
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

        builder.ConfigureTestServices(services =>
            services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, Tokens.Trust));
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

    /// <summary>Sends the request with a valid access token for <paramref name="user"/>; <c>null</c> sends it anonymously.</summary>
    public Task<HttpResponseMessage> SendAsync(HttpMethod method, string route, HttpContent? content, TestUser? user) =>
        SendWithTokenAsync(method, route, content, user == null ? null : Tokens.Issue(user));

    public async Task<HttpResponseMessage> SendWithTokenAsync(HttpMethod method, string route, HttpContent? content, string? accessToken)
    {
        var message = new HttpRequestMessage
        {
            Method = method,
            RequestUri = new Uri(route, UriKind.Relative),
            Content = content
        };
        if (accessToken != null)
        {
            message.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken);
        }

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
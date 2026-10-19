using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NET.Backend.Blueprint.Infrastructure.DataAccess;
using NET.Backend.Blueprint.Minimal.Api;
using Testcontainers.PostgreSql;
using Xunit;

namespace NET.Backend.Blueprint.Minimal.Integration.xUnit.Tests.Environment;

public class Fixture : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _database = new PostgreSqlBuilder("postgres:18-alpine").Build();

    public TestTokenIssuer Tokens { get; } = new();

    private HttpClient Client { get; set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("RateLimiting:PermitLimit", int.MaxValue.ToString());
        builder.UseSetting("ConnectionStrings:Database", _database.GetConnectionString());
        builder.ConfigureTestServices(services =>
            services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, Tokens.Trust));
    }

    public async Task InitializeAsync()
    {
        await _database.StartAsync();

        Client = CreateClient();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        Client.Dispose();
        await _database.DisposeAsync();
    }

    /// <summary>Sends the request with a valid access token for <paramref name="user"/>; <c>null</c> sends it anonymously.</summary>
    public Task<HttpResponseMessage> SendAsync(HttpMethod method, string route, TestUser? user, object? body = null) =>
        SendWithTokenAsync(method, route, user == null ? null : Tokens.Issue(user), body);

    public Task<HttpResponseMessage> SendWithTokenAsync(HttpMethod method, string route, string? accessToken, object? body = null)
    {
        var message = new HttpRequestMessage(method, new Uri(route, UriKind.Relative))
        {
            Content = body == null ? null : JsonContent.Create(body, body.GetType())
        };
        if (accessToken != null)
        {
            message.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken);
        }

        return Client.SendAsync(message);
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

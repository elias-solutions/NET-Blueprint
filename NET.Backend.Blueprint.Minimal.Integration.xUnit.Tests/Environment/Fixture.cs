using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NET.Backend.Blueprint.Infrastructure.DataAccess;
using NET.Backend.Blueprint.Minimal.Api;
using NET.Backend.Blueprint.Web.Authorization;
using NSubstitute;
using Testcontainers.PostgreSql;
using Xunit;

namespace NET.Backend.Blueprint.Minimal.Integration.xUnit.Tests.Environment;

public class Fixture : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _database = new PostgreSqlBuilder("postgres:18-alpine").Build();

    public IUserService UserService { get; } = Substitute.For<IUserService>();

    private HttpClient Client { get; set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("RateLimiting:PermitLimit", int.MaxValue.ToString());
        builder.UseSetting("ConnectionStrings:Database", _database.GetConnectionString());
        builder.ConfigureTestServices(services => services.AddScoped(_ => UserService));
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

    public Task<HttpResponseMessage> SendAsync(HttpMethod method, string route, User? user, object? body = null)
    {
        UserService.GetCurrentUser().Returns(user);

        var message = new HttpRequestMessage(method, new Uri(route, UriKind.Relative))
        {
            Content = body == null ? null : JsonContent.Create(body, body.GetType())
        };

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

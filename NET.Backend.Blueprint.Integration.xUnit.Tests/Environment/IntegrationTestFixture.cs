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

public class IntegrationTestFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    public IUserService UserService { get; }
    public HttpClient Client { get; private set; } = null!;
    private readonly string _databaseName;
    public IDatabaseResetProvider? DatabaseResetProvider { get; private set; }

    public IntegrationTestFixture()
    {
        _databaseName = Guid.NewGuid().ToString();
        UserService = Substitute.For<IUserService>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Add configuration sources
            config.AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
        });

        builder
            .ConfigureTestServices(services =>
            {
                // Remove existing DbContext registration
                var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<BlueprintDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Use unique database name
                services.AddDbContextFactory<BlueprintDbContext>(options =>
                    options.UseInMemoryDatabase(_databaseName));

                // Optional: Set up a database reset provider if needed
                DatabaseResetProvider = new DatabaseInMemoryResetProvider(
                    services.BuildServiceProvider().GetRequiredService<IDbContextFactory<BlueprintDbContext>>());
                services.AddSingleton(_ => DatabaseResetProvider);

                services.AddScoped(_ => UserService);
                services.AddAuthentication("TestAuthentication")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("TestAuthentication", null);
            });
    }

    public Task InitializeAsync()
    {
        Client = CreateClient();
        Client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        Client?.Dispose();
        return Task.CompletedTask;
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
}
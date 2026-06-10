using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NET.Backend.Blueprint.Api.Authorization;
using NET.Backend.Blueprint.Api.DataAccess;
using NSubstitute;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;

public class Fixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    public IUserService UserService { get; } = Substitute.For<IUserService>();
    private HttpClient Client { get; set; } = null!;
    
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
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BlueprintDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContextFactory<BlueprintDbContext>(options => options.UseInMemoryDatabase("Database"));
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

    Task IAsyncLifetime.DisposeAsync()
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

    public async Task ResetDatabaseAsync()
    {
        var contextFactory = Services.GetRequiredService<IDbContextFactory<BlueprintDbContext>>();
        await using var dbContext = await contextFactory.CreateDbContextAsync();

        dbContext.Addresses.RemoveRange(dbContext.Addresses);
        dbContext.Persons.RemoveRange(dbContext.Persons);
        await dbContext.SaveChangesAsync();
    }
}
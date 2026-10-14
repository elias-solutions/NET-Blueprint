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
using Xunit;

namespace NET.Backend.Blueprint.Minimal.Integration.xUnit.Tests.Environment;

public class Fixture : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly string _databaseName = $"MinimalTestDatabase-{Guid.NewGuid()}";

    public IUserService UserService { get; } = Substitute.For<IUserService>();

    private HttpClient Client { get; set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("RateLimiting:PermitLimit", int.MaxValue.ToString());
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<BlueprintDbContext>>();
            services.AddDbContextFactory<BlueprintDbContext>(options => options.UseInMemoryDatabase(_databaseName));
            services.AddScoped(_ => UserService);
        });
    }

    public Task InitializeAsync()
    {
        Client = CreateClient();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        return Task.CompletedTask;
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        Client.Dispose();
        return Task.CompletedTask;
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

internal static class ServiceCollectionTestExtensions
{
    public static void RemoveAll<TService>(this IServiceCollection services)
    {
        var descriptors = services.Where(d => d.ServiceType == typeof(TService)).ToList();
        descriptors.ForEach(descriptor => services.Remove(descriptor));
    }
}

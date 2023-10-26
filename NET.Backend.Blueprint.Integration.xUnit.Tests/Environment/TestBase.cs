using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NET.Backend.Blueprint.Api;
using NET.Backend.Blueprint.Api.Authorization;
using NSubstitute;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;

public abstract class TestBase : WebApplicationFactory<Startup>, IAsyncLifetime
{
    public IUserService UserService { get; }
    public PostgresDbResetProvider PostgresDbResetProvider { get; }
    protected HttpClient Client { get; }
    
    protected TestBase()
    {
        UserService = Substitute.For<IUserService>();
        Client = CreateClient();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        PostgresDbResetProvider = new PostgresDbResetProvider();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {            
        builder
            .ConfigureTestServices(services =>
            {
                services.AddScoped(_ => UserService);
                services.AddAuthentication("TestAuthentication")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("TestAuthentication", null);
            });

        builder.UseEnvironment("IntegrationTestPostgres");
    }

    public async Task InitializeAsync() => await PostgresDbResetProvider.InitializeAsync();

    async Task IAsyncLifetime.DisposeAsync() => await PostgresDbResetProvider.DisposeDbConnectionAsync();
}
using System.Net.Http.Headers;
using System.Net.Mime;
using BIT.NET.Backend.Blueprint.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;

public abstract class TestBase : WebApplicationFactory<Startup>, IAsyncLifetime
{
    private const string ConnectionString = "Host=localhost; Database=BlueprintDatabase; Username=dev; Password=dev";
    public IUserService UserService { get; }
    public RespawnerHelper RespawnerHelper { get; }
    protected HttpClient Client { get; }
    
    protected TestBase()
    {
        UserService = Substitute.For<IUserService>();
        Client = CreateClient();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        RespawnerHelper = new RespawnerHelper(ConnectionString);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {            
        builder.ConfigureTestServices(services =>
        {
            services.AddScoped(_ => UserService);
            services.AddAuthentication("TestAuthentication")
                .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("TestAuthentication", null);
        });
    }

    public async Task InitializeAsync() => await RespawnerHelper.InitializeRespawner();

    async Task IAsyncLifetime.DisposeAsync() => await RespawnerHelper.CloseDbConnectionRespawner();
}
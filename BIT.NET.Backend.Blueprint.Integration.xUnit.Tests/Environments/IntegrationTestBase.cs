using BIT.NET.Backend.Blueprint.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;

public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactory<Startup>>
{
    protected WebApplicationFactory<Startup> Factory { get; }
    protected IUserService UserService { get; }

    protected IntegrationTestBase(WebApplicationFactory<Startup> factory)
    {
        UserService = Substitute.For<IUserService>();

        Factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddScoped(_ => UserService);
                services
                    .AddAuthentication("TestAuthentication")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("TestAuthentication", null);
            });
        });
    }
}
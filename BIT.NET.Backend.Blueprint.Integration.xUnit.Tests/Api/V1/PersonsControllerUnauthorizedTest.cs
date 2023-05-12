using System.Net;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1;

public class PersonsControllerUnauthorizedTest : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;
    private const string Route = "/api/v1/persons";

    public PersonsControllerUnauthorizedTest(WebApplicationFactory<Startup> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services
                    .AddAuthentication("TestAuthentication")
                    .AddScheme<AuthenticationSchemeOptions, UnauthorizedTestAuthenticationHandler>("TestAuthentication", null);
            });
        });
    }

    [Fact]
    public async Task PersonController_Post_Unauthorized()
    {
        using var client = _factory.CreateClient();

        var response = await client.PostAsync(Route, new StringContent(string.Empty));
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PersonController_Delete_Unauthorized()
    {
        using var client = _factory.CreateClient();

        var response = await client.DeleteAsync(Route);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PersonController_GetAll_Unauthorized()
    {
        using var client = _factory.CreateClient();

        var response = await client.GetAsync(Route);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PersonController_Get_Unauthorized()
    {
        using var client = _factory.CreateClient();

        var response = await client.GetAsync($"{Route}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
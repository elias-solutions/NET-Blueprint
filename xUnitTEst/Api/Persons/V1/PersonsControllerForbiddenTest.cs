using System.Net;
using System.Text;
using System.Text.Json;
using BIT.NET.Backend.Blueprint.Integration.Tests.Environments;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.Tests.Api.Persons.V1;

public class PersonsControllerForbiddenTest : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;
    private readonly CreatePersonRequest _request;
    private const string Route = "/api/v1/persons";

    public PersonsControllerForbiddenTest(WebApplicationFactory<Startup> factory)
    {
        _factory = factory;
        _request = new CreatePersonRequest("Jonas", "Elias", DateTime.Now);
    }

    [Fact]
    public async Task PersonController_Post_Forbidden()
    {
        using var client = BuildAuthenticatedClient();

        var httpContent = new StringContent(JsonSerializer.Serialize(_request), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(Route, httpContent);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PersonController_Delete_Forbidden()
    {
        using var client = BuildAuthenticatedClient();

        var response = await client.DeleteAsync(Route);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PersonController_GetAll_Forbidden()
    {
        using var client = BuildAuthenticatedClient();

        var response = await client.GetAsync(Route);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PersonController_Get_Forbidden()
    {
        using var client = BuildAuthenticatedClient();

        var response = await client.GetAsync($"{Route}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private HttpClient BuildAuthenticatedClient()
    {
        return _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("TestAuthentication")
                    .AddScheme<AuthenticationSchemeOptions, StandardTestAuthenticationHandler>("TestAuthentication", null);
            });
        }).CreateClient();
    }
}
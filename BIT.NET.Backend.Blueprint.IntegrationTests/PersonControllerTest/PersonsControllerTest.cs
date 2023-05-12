using System.Net;
using System.Text;
using System.Text.Json;
using BIT.NET.Backend.Blueprint.Entities;
using BIT.NET.Backend.Blueprint.Extensions;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.Tests.PersonControllerTest;

public class PersonsControllerTest : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;
    private const string Route = "/api/v1/persons";
    private readonly CreatePersonRequest _request;

    public PersonsControllerTest(WebApplicationFactory<Startup> factory)
    {
        _factory = factory;
        _request = new CreatePersonRequest("Jonas", "Elias", DateTime.Now);
    }

    [Fact]
    public async Task PersonsController_Ok()
    {
        using var client = BuildAuthenticatedClient();

        var httpContent = new StringContent(JsonSerializer.Serialize(_request), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(Route, httpContent);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var addedPerson = await response.Content.ReadAsync<PersonDto>();

        response = await client.GetAsync(Route);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var personsResult = await response.Content.ReadAsync<IEnumerable<Person>>();
        personsResult.Should().BeEquivalentTo(new[] { addedPerson });
        
        response = await client.GetAsync($"{Route}/{addedPerson.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var personResult = await response.Content.ReadAsync<Person>();
        personResult.Should().BeEquivalentTo(addedPerson);
        
        response = await client.DeleteAsync($"{Route}/{addedPerson.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private HttpClient BuildAuthenticatedClient()
    {
        return _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("TestAuthentication")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("TestAuthentication", null);
            });
        }).CreateClient();
    }
}
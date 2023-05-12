using System.Net;
using System.Text;
using System.Text.Json;
using BIT.NET.Backend.Blueprint.Extensions;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1;

public class PersonsControllerTest : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;
    private const string Route = "/api/v1/persons";
    private readonly CreatePersonRequest _request;

    public PersonsControllerTest(WebApplicationFactory<Startup> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services
                    .AddAuthentication("TestAuthentication")
                    .AddScheme<AuthenticationSchemeOptions, AdminTestAuthenticationHandler>("TestAuthentication", null);
            });
        });

        _request = new CreatePersonRequest("Jonas", "Elias", DateTime.Now);
    }

    [Fact]
    public async Task PersonsController_Ok()
    {
        using var client = _factory.CreateClient();

        var httpContent = new StringContent(JsonSerializer.Serialize(_request), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(Route, httpContent);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var addedPerson = await response.Content.ReadAsync<PersonDto>();
        addedPerson.Id.Should().NotBe(Guid.Empty);
        addedPerson.FirstName.Should().Be(_request.FirstName);
        addedPerson.LastName.Should().Be(_request.LastName);
        addedPerson.Birthday.Should().Be(_request.Birthday);
        addedPerson.Created.Should().NotBe(DateTime.MinValue);
        addedPerson.CreatedBy.Should().NotBe(Guid.Empty);
        addedPerson.Modified.Should().Be(DateTime.MinValue);
        addedPerson.ModifiedBy.Should().Be(Guid.Empty);

        response = await client.GetAsync(Route);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var personsResult = await response.Content.ReadAsync<IEnumerable<PersonDto>>();
        personsResult.Should().BeEquivalentTo(new[] { addedPerson });

        response = await client.GetAsync($"{Route}/{addedPerson.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var personResult = await response.Content.ReadAsync<PersonDto>();
        personResult.Should().BeEquivalentTo(addedPerson);

        response = await client.DeleteAsync($"{Route}/{addedPerson.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        response = await client.GetAsync(Route);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        personsResult = await response.Content.ReadAsync<IEnumerable<PersonDto>>();
        personsResult.Should().BeEmpty();
    }
}

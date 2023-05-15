using System.Net;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1;

public class PersonsControllerUnauthorizedTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";

    public PersonsControllerUnauthorizedTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
    }

    [Fact]
    public async Task PersonController_Post_Unauthorized()
    {
        using var client = Factory.CreateClient();

        var response = await client.PostAsync(Route, new StringContent(string.Empty));
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PersonController_Delete_Unauthorized()
    {
        using var client = Factory.CreateClient();

        var response = await client.DeleteAsync(Route);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PersonController_GetAll_Unauthorized()
    {
        using var client = Factory.CreateClient();

        var response = await client.GetAsync(Route);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PersonController_Get_Unauthorized()
    {
        using var client = Factory.CreateClient();

        var response = await client.GetAsync($"{Route}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
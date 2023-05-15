using System.Net;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1;

public class PersonsControllerForbiddenTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";

    public PersonsControllerForbiddenTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
        UserService.GetCurrentUser().Returns(TestUsers.Standard);
    }

    [Fact]
    public async Task PersonController_Post_Forbidden()
    {
        using var client = Factory.CreateClient();
        var response = await client.PostAsync(Route, new StringContent(string.Empty));
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PersonController_Delete_Forbidden()
    {
        using var client = Factory.CreateClient();
        var response = await client.DeleteAsync(Route);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PersonController_GetAll_Forbidden()
    {
        using var client = Factory.CreateClient();
        var response = await client.GetAsync(Route);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PersonController_Get_Forbidden()
    {
        using var client = Factory.CreateClient();
        var response = await client.GetAsync($"{Route}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
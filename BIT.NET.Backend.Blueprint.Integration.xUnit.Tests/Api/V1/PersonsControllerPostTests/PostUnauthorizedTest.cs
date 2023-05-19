using System.Net;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPostTests;

public class PostUnauthorizedTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";

    public PostUnauthorizedTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
    }

    [Fact]
    public async Task PersonController_Post_Unauthorized()
    {
        var response = await Client.PostAsync(Route, new StringContent(string.Empty));
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
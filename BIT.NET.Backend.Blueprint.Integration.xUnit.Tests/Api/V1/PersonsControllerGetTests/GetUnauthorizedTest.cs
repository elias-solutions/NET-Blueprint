using System.Net;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetTests;

public class GetUnauthorizedTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";

    public GetUnauthorizedTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task PersonController_Get_Unauthorized()
    {
        var response = await Client.GetAsync($"{Route}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
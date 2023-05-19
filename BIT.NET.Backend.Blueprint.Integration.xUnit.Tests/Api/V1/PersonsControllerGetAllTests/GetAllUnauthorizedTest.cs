using System.Net;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetAllTests;

public class GetAllUnauthorizedTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";

    public GetAllUnauthorizedTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
    }

    [Fact]
    public async Task PersonController_GetAll_Unauthorized()
    {
        var response = await Client.GetAsync(Route);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
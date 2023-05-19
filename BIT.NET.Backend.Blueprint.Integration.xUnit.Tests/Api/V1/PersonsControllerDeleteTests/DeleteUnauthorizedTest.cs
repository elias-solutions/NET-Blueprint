using System.Net;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerDeleteTests;

public class DeleteUnauthorizedTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";

    public DeleteUnauthorizedTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
    }

    [Fact]
    public async Task PersonController_Delete_Unauthorized()
    {
        var response = await Client.DeleteAsync($"{Route}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
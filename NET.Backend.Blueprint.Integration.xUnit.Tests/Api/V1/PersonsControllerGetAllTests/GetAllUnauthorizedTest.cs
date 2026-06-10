using System.Net;
using FluentAssertions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetAllTests;

[Collection(nameof(SharedTestCollection))]
public class GetAllUnauthorizedTest(Fixture fixture)
{
    private const string Route = "/api/v1/persons";

    [Fact]
    public async Task PersonController_GetAll_Unauthorized()
    {
        var response = await fixture.SendAsync(HttpMethod.Get, Route, null, null);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
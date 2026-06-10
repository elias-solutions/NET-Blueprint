using System.Net;
using FluentAssertions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetTests;

[Collection(nameof(SharedTestCollection))]
public class GetUnauthorizedTest(Fixture fixture)
{
    private const string Route = "/api/v1/persons";

    [Fact]
    public async Task PersonController_Get_Unauthorized()
    {
        var response = await fixture.SendAsync(HttpMethod.Get, $"{Route}/{Guid.NewGuid()}", null, null);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
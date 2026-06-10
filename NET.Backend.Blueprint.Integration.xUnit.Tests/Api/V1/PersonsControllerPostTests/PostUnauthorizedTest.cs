using System.Net;
using FluentAssertions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPostTests;

[Collection(nameof(SharedTestCollection))]
public class PostUnauthorizedTest(IntegrationTestFixture fixture)
{
    private const string Route = "/api/v1/persons";

    [Fact]
    public async Task PersonController_Post_Unauthorized()
    {
        var response = await fixture.SendAsync(HttpMethod.Post, Route, null, null);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
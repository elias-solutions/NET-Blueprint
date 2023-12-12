using System.Net;
using NET.Backend.Blueprint.Integration.xUnit.Tests.SQlite.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.SQlite.Api.V1.PersonsControllerPostTests;

[Collection(nameof(SharedTestCollection))]
public class PostUnauthorizedTest
{
    private readonly IntegrationTestFixture _fixture;
    private const string Route = "/api/v1/persons";

    public PostUnauthorizedTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task PersonController_Post_Unauthorized()
    {
        var response = await _fixture.SendAnonymousAsync(Route, HttpMethod.Post);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
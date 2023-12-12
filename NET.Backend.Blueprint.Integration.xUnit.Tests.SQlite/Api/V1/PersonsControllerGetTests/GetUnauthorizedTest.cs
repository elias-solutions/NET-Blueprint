using System.Net;
using NET.Backend.Blueprint.Integration.xUnit.Tests.SQlite.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.SQlite.Api.V1.PersonsControllerGetTests;

[Collection(nameof(SharedTestCollection))]
public class GetUnauthorizedTest
{
    private readonly IntegrationTestFixture _fixture;
    private const string Route = "/api/v1/persons";

    public GetUnauthorizedTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task PersonController_Get_Unauthorized()
    {
        var response = await _fixture.SendAnonymousAsync($"{Route}/{Guid.NewGuid()}", HttpMethod.Get);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
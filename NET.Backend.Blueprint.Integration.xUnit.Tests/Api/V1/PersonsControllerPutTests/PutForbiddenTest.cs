using System.Net;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPutTests;

[Collection(nameof(SharedTestCollection))]
public class PutForbiddenTest(Fixture fixture)
{
    private const string Route = "/api/v1/persons";

    [Fact]
    public async Task PersonController_Put_Reader_Forbidden()
    {
        var response = await fixture.SendAsync(HttpMethod.Put, $"{Route}/{Guid.NewGuid()}", null, TestUsers.Reader);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}

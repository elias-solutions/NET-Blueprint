using System.Net;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPostTests;

[Collection(nameof(SharedTestCollection))]
public class PostForbiddenTest(Fixture fixture)
{
    private const string Route = "/api/v1/persons";

    [Fact]
    public async Task PersonController_Post_Forbidden()
    {
        var response = await fixture.SendAsync(HttpMethod.Post, Route, null, TestUsers.Standard);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}

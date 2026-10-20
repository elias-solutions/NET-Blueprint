using System.Net;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPostTests;

[Collection(nameof(SharedTestCollection))]
public class PostWriterOkTest(Fixture fixture)
{
    private const string Route = "/api/v1/persons";
    private readonly EmbeddedJsonResourceProvider _jsonResourceProvider = new(typeof(PostWriterOkTest).Namespace!);

    [Fact]
    public async Task PersonsController_Post_Writer_Ok()
    {
        var content = await _jsonResourceProvider.CreateHttpContentByResourceAsync("Post_Person_Request.json");
        var response = await fixture.SendAsync(HttpMethod.Post, Route, content, TestUsers.Writer);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}

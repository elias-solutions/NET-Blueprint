using System.Net;
using NET.Backend.Blueprint.Application.Model.Queries;
using NET.Backend.Blueprint.Extensions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetTests;

[Collection(nameof(SharedTestCollection))]
public class GetStandardUserOkTest(Fixture fixture) : IAsyncLifetime
{
    private const string Route = "/api/v1/persons";
    private readonly EmbeddedJsonResourceProvider _jsonResourceProvider = new(typeof(GetStandardUserOkTest).Namespace!);

    public async Task InitializeAsync() => await fixture.ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonController_Get_StandardUser_Ok()
    {
        var content = await _jsonResourceProvider.CreateHttpContentByResourceAsync("Post_Person_Request.json");
        var created = await (await fixture.SendAsync(HttpMethod.Post, Route, content, TestUsers.Admin))
            .Content.ReadAsync<GetPersonResponse>();

        var response = await fixture.SendAsync(HttpMethod.Get, $"{Route}/{created.Id}", null, TestUsers.Standard);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}

using FluentAssertions;
using NET.Backend.Blueprint.Api.Model.Queries;
using NET.Backend.Blueprint.Extensions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using System.Net;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetTests;

[Collection(nameof(SharedTestCollection))]
public class GetOkTest(Fixture fixture) : IAsyncLifetime
{
    private const string Route = "/api/v1/persons";
    private readonly EmbeddedJsonResourceProvider _jsonResourceProvider = new(typeof(GetOkTest).Namespace);

    public async Task InitializeAsync() => await fixture.ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonController_Get_Ok()
    {
        var content = await _jsonResourceProvider.CreateHttpContentByResourceAsync("Post_Person_Request.json");
        var response = await fixture.SendAsync(HttpMethod.Post, Route, content, TestUsers.Admin);
        var getPersonResponse = await response.Content.ReadAsync<GetPersonResponse>();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        response = await fixture.SendAsync(HttpMethod.Get, $"{Route}/{getPersonResponse.Id}", null, TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var expected = await _jsonResourceProvider.CreateObjectByResourceAsync<GetPersonResponse>("Get_Person_Response.json");
        var result = await response.Content.ReadAsync<GetPersonResponse>();
        result.Should().BeEquivalentTo(expected, options => options
            .Excluding(x => x.Id)
            .Excluding(x => x.Version)
            .For(x => x.Addresses).Exclude(x => x.Id)
            .For(x => x.Addresses).Exclude(x => x.Version)
        );
    }
}
using FluentAssertions;
using NET.Backend.Blueprint.Api.CQRS.Commands;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerDeleteTests;

[Collection(nameof(SharedTestCollection))]
public class DeleteOkTest(Fixture fixture) : IAsyncLifetime
{
    private const string Route = "/api/v1/persons";
    private readonly EmbeddedJsonResourceProvider _jsonResourceProvider = new(typeof(DeleteOkTest).Namespace!);

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonsController_Delete_Ok()
    {
        var content = await _jsonResourceProvider.CreateHttpContentByResourceAsync("Post_Person_Request.json");
        var response = await fixture.SendAsync(HttpMethod.Post, Route, content, TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var personResponse = await response.Content.ReadFromJsonAsync<CreatePersonResponse>();

        response = await fixture.SendAsync(HttpMethod.Delete, $"{Route}/{personResponse!.Id}", null, TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
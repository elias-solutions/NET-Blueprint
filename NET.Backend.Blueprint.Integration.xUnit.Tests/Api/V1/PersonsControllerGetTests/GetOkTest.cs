using System.Net;
using FluentAssertions;
using NET.Backend.Blueprint.Api.Model.Commands;
using NET.Backend.Blueprint.Api.Model.Queries;
using NET.Backend.Blueprint.Extensions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetTests;

[Collection(nameof(SharedTestCollection))]
public class GetOkTest : IAsyncLifetime
{
    private const string Route = "/api/v1/persons";
    private readonly IntegrationTestFixture _fixture;
    private readonly EmbeddedJsonResourceProvider _jsonResourceProvider;
    private GetPersonResponse _dbGetPerson = default!;

    public GetOkTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _jsonResourceProvider = new EmbeddedJsonResourceProvider(GetType().Namespace!);
    }
    
    public async Task InitializeAsync()
    {
        var content = await _jsonResourceProvider.CreateHttpContentByResourceAsync("Post_Person_Request.json");
        var response = await _fixture.SendAsync(HttpMethod.Post, Route, content, TestUsers.Admin);
        _dbGetPerson = await response.Content.ReadAsync<GetPersonResponse>(); 
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonController_Get_Ok()
    {
        var response = await _fixture.SendAsync(HttpMethod.Get, $"{Route}/{_dbGetPerson.Id}", null, TestUsers.Admin);
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
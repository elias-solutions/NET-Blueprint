using FluentAssertions;
using NET.Backend.Blueprint.Api.CQRS.Commands;
using NET.Backend.Blueprint.Api.Model.Queries;
using NET.Backend.Blueprint.Extensions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPostTests;

[Collection(nameof(SharedTestCollection))]
public class PostOkTest
{
    private const string Route = "/api/v1/persons";
    private readonly IntegrationTestFixture _fixture;
    private readonly EmbeddedJsonResourceProvider _jsonResourceProvider;

    public PostOkTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _jsonResourceProvider = new EmbeddedJsonResourceProvider(GetType().Namespace!);
    }

    [Fact]
    public async Task PersonsController_Ok()
    {
        var content = await _jsonResourceProvider.CreateHttpContentByResourceAsync("Post_Person_Request.json");
        var response = await _fixture.SendAsync(HttpMethod.Post, Route, content, TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var createPersonResponse = await response.Content.ReadFromJsonAsync<CreatePersonResponse>();

        response = await _fixture.SendAsync(HttpMethod.Get, $"{Route}/{createPersonResponse!.Id}", null, TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dbPerson = await response.Content.ReadAsync<GetPersonResponse>();
        
        var expectedPerson =  await _jsonResourceProvider.CreateObjectByResourceAsync<GetPersonResponse>("Post_Person_Response.json");
        dbPerson.Should().BeEquivalentTo(expectedPerson, options => options
            .Excluding(entity => entity.Id)
            .Excluding(entity => entity.Version)
            .For(entity => entity.Addresses).Exclude(entity => entity.Id)
            .For(entity => entity.Addresses).Exclude(entity => entity.Version)
        );
    }
}

using System.Net;
using FluentAssertions;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Extensions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Extensions;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPostTests;

[Collection(nameof(SharedTestCollection))]
public class PostOkTest : IAsyncLifetime
{
    private const string Route = "/api/v1/persons";
    private readonly IntegrationTestFixture _fixture;
    private readonly EmbeddedJsonResourceProvider _jsonResourceProvider;

    public PostOkTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _jsonResourceProvider = new EmbeddedJsonResourceProvider(GetType().Namespace!);
    }

    public async Task InitializeAsync() => await _fixture.PostgresDbResetProvider.ResetAsync();
    
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonsController_Ok()
    {
        var content = await _jsonResourceProvider.CreateHttpContentByResourceAsync("Post_Person_Request.json");
        var response = await _fixture.SendAsync(TestUsers.Admin, Route, HttpMethod.Post, content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dbPerson = await response.Content.ReadAsync<PersonDto>();
        
        var expectedPerson =  await _jsonResourceProvider.CreateObjectByResourceAsync<PersonDto>("Post_Person_Response.json");
        dbPerson.Should().BeEquivalentTo(expectedPerson, options => options
            .Excluding(entity => entity.Id)
            .Excluding(entity => entity.Created)
            .Excluding(entity => entity.CreatedBy)
            .Excluding(entity => entity.Modified)
            .Excluding(entity => entity.ModifiedBy)
            .Excluding(entity => entity.Version)
            .For(entity => entity.Addresses).Exclude(entity => entity.Id)
            .For(entity => entity.Addresses).Exclude(entity => entity.Created)
            .For(entity => entity.Addresses).Exclude(entity => entity.CreatedBy)
            .For(entity => entity.Addresses).Exclude(entity => entity.Modified)
            .For(entity => entity.Addresses).Exclude(entity => entity.ModifiedBy));
    }
}

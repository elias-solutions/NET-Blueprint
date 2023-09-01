using System.Net;
using FluentAssertions;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Extensions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Extensions;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerUpdateTests;

[Collection(nameof(SharedTestCollection))]
public class PutOkTest : IAsyncLifetime
{
    private const string Route = "/api/v1/persons";
    private readonly IntegrationTestFixture _fixture;
    private readonly EmbeddedJsonResourceProvider _jsonResourceProvider;
    private PersonDto? _dbPerson;

    public PutOkTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _jsonResourceProvider = new EmbeddedJsonResourceProvider(GetType().Namespace!);
    }

    public async Task InitializeAsync()
    {
        await _fixture.PostgresDbResetProvider.ResetAsync();

        var content = await _jsonResourceProvider.CreateHttpContentByResourceAsync("Post_Person_Request.json");
        var response = await _fixture.PostAsync(TestUsers.Admin, Route, content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        _dbPerson = await response.Content.ReadAsync<PersonDto>();
    }
    
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonsController_Ok()
    {
        var expectedPerson = await _jsonResourceProvider.CreateObjectByResourceAsync<PersonDto>("Put_Person_Request.json") with { Id = _dbPerson!.Id, Version = _dbPerson.Version };
        var response = await _fixture.PutAsync(TestUsers.Admin, $"{Route}/{_dbPerson!.Id}", expectedPerson.ToJson().ToStringContent());
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedPerson = await response.Content.ReadAsync<PersonDto>();
        
        updatedPerson.Should().BeEquivalentTo(expectedPerson, options => options
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
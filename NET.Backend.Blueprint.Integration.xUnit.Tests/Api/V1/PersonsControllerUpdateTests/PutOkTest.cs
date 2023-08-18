using System.Net;
using FluentAssertions;
using NET.Backend.Blueprint.Extensions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using NET.Backend.Blueprint.Model;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerUpdateTests;

[Collection(nameof(SharedTestCollection))]
public class PutOkTest : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    private PersonDto? _dbPerson;
    private const string Route = "/api/v1/persons";

    public PutOkTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        var response = await _fixture.PostAsync(TestUsers.Admin, Route, "Post_Person_Request.json");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        _dbPerson = await response.Content.ReadAsync<PersonDto>();
    }
    
    public async Task DisposeAsync() => await _fixture.RespawnerHelper.ResetAsync();

    [Fact]
    public async Task PersonsController_Ok()
    {
        var expectedPerson = await _fixture.CreateExpected<PersonDto>("Put_Person_Request.json") with { Id = _dbPerson!.Id };

        var response = await _fixture.PutAsync(TestUsers.Admin, $"{Route}/{_dbPerson!.Id}", expectedPerson);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedPerson = await response.Content.ReadAsync<PersonDto>();
        
        updatedPerson.Should().BeEquivalentTo(expectedPerson, options => options
            .Excluding(entity => entity.Id)
            .Excluding(entity => entity.Created)
            .Excluding(entity => entity.CreatedBy)
            .Excluding(entity => entity.Modified)
            .Excluding(entity => entity.ModifiedBy)
            .For(entity => entity.Addresses).Exclude(entity => entity.Id)
            .For(entity => entity.Addresses).Exclude(entity => entity.Created)
            .For(entity => entity.Addresses).Exclude(entity => entity.CreatedBy)
            .For(entity => entity.Addresses).Exclude(entity => entity.Modified)
            .For(entity => entity.Addresses).Exclude(entity => entity.ModifiedBy));
    }
}

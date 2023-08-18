using System.Net;
using FluentAssertions;
using NET.Backend.Blueprint.Extensions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Extensions;
using NET.Backend.Blueprint.Model;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPostTests;

[Collection(nameof(SharedTestCollection))]
public class PostOkTest : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    private const string Route = "/api/v1/persons";

    public PostOkTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }


    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _fixture.RespawnerHelper.ResetAsync();

    [Fact]
    public async Task PersonsController_Ok()
    {
        var response = await _fixture.PostAsync(TestUsers.Admin, Route, "Post_Person_Request.json");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dbPerson = await response.Content.ReadAsync<PersonDto>();
        
        var expectedPerson =  (await "Post_Person_Request.json".ToJsonStringContentAsync()).ReadFromJson<PersonDto>();
        dbPerson.Should().BeEquivalentTo(expectedPerson, options => options
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

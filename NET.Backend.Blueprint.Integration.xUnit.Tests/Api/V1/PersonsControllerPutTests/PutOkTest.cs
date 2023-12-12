using System.Net;
using FluentAssertions;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Extensions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Extensions;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPutTests;

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
        await _fixture.DatabaseResetProvider.ResetAsync();

        var content = await _jsonResourceProvider.CreateHttpContentByResourceAsync("Post_Person_Request.json");
        var response = await _fixture.SendAsync(TestUsers.Admin, Route, HttpMethod.Post, content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        _dbPerson = await response.Content.ReadAsync<PersonDto>();
    }
    
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonsController_Ok()
    {
        var expectedPerson = await _jsonResourceProvider.CreateObjectByResourceAsync<PersonDto>("Put_Person_Request.json") with
        {
            Id = _dbPerson!.Id, 
            Addresses = _dbPerson.Addresses.Select(address => address with { Id = address.Id }),
            Version = _dbPerson.Version,
        };
        
        var response = await _fixture.SendAsync(TestUsers.Admin, $"{Route}/{_dbPerson!.Id}", HttpMethod.Put, expectedPerson.ToJson().ToStringContent());
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        response = await _fixture.SendAsync(TestUsers.Admin, $"{Route}/{_dbPerson.Id}", HttpMethod.Get);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var person = await response.Content.ReadAsync<PersonDto>();
        
        person.Should().BeEquivalentTo(expectedPerson, options => options
            .Excluding(x => x.Modified)
            .Excluding(x => x.ModifiedBy)
            .Excluding(x => x.Version)
            .Excluding(x => x.Created)
            .Excluding(x => x.CreatedBy)
            .For(x => x.Addresses).Exclude(x => x.Modified)
            .For(x => x.Addresses).Exclude(x => x.ModifiedBy)
            .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(1000)))
            .WhenTypeIs<DateTimeOffset>());
    }
}
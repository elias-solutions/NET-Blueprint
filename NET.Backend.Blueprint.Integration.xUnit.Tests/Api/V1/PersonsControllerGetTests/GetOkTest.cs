using System.Net;
using FluentAssertions;
using NET.Backend.Blueprint.Api.Model;
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
    private PersonDto _dbPerson = default!;

    public GetOkTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _jsonResourceProvider = new EmbeddedJsonResourceProvider(GetType().Namespace!);
    }
    
    public async Task InitializeAsync()
    {
        await _fixture.PostgresDbResetProvider.ResetAsync();

        var content = await _jsonResourceProvider.CreateHttpContentByResourceAsync("Post_Person_Request.json");
        var response = await _fixture.PostAsync(TestUsers.Admin, Route, content);
        _dbPerson = await response.Content.ReadAsync<PersonDto>(); 
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonController_Get_Ok()
    {
        var response = await _fixture.GetAsync(TestUsers.Admin, $"{Route}/{_dbPerson.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsync<PersonDto>();
        result.Should().BeEquivalentTo(_dbPerson, options => options
            .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(1000)))
            .WhenTypeIs<DateTimeOffset>());
    }
}
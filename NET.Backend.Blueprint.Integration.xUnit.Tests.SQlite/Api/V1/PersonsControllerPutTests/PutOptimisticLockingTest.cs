using System.Net;
using NET.Backend.Blueprint.Integration.xUnit.Tests.SQlite.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.SQlite.Api.V1.PersonsControllerPutTests;

[Collection(nameof(SharedTestCollection))]
public class PutOptimisticLockingTest : IAsyncLifetime
{
    private const string Route = "/api/v1/persons";
    private readonly IntegrationTestFixture _fixture;
    private readonly EmbeddedJsonResourceProvider _jsonResourceProvider;
    private PersonDto? _dbPerson;

    public PutOptimisticLockingTest(IntegrationTestFixture fixture)
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
        var expectedPerson = await _jsonResourceProvider.CreateObjectByResourceAsync<PersonDto>("Put_Person_Request.json") with { Id = _dbPerson!.Id, Version = Guid.NewGuid() };
        var response = await _fixture.SendAsync(TestUsers.Admin, $"{Route}/{_dbPerson!.Id}", HttpMethod.Put, expectedPerson.ToJson().ToStringContent());
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var problemDetails = await response.Content.ReadAsync<ProblemDetails>();
        problemDetails.Should().BeEquivalentTo(new ProblemDetails
        {
            Status = (int)HttpStatusCode.BadRequest,
            Title = "BadRequest - Entity version conflict",
            Detail = "Entity has been updated through other user."
        });
    }
}
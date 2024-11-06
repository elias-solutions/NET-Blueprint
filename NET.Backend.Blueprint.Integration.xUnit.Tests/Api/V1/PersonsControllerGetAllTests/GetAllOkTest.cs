using System.Net;
using FluentAssertions;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Extensions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetAllTests;

[Collection(nameof(SharedTestCollection))]
public class GetAllOkTest : IAsyncLifetime
{
    private const string Route = "/api/v1/persons";
    private readonly IntegrationTestFixture _fixture;
    private readonly EmbeddedJsonResourceProvider _jsonResourceProvider;
    private GetPersonRequest _dbGetPerson = default!;

    public GetAllOkTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _jsonResourceProvider = new EmbeddedJsonResourceProvider(GetType().Namespace!);
    }
    public async Task InitializeAsync()
    {
        await _fixture.DatabaseResetProvider.ResetAsync();
        var content = await _jsonResourceProvider.CreateHttpContentByResourceAsync("Post_Person_Request.json");
        var response = await _fixture.SendAsync(HttpMethod.Post, Route, content, TestUsers.Admin);  
        _dbGetPerson = await response.Content.ReadAsync<GetPersonRequest>(); 
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonController_GetAll_Ok()
    {
        var response = await _fixture.SendAsync(HttpMethod.Get, Route, TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsync<IEnumerable<GetPersonRequest>>();
        result.Should().BeEquivalentTo(new[] { _dbGetPerson }, options => options
            .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(1000)))
            .WhenTypeIs<DateTimeOffset>());
    }
}
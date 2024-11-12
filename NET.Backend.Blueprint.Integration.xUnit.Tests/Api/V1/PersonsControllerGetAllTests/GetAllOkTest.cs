using System.Net;
using FluentAssertions;
using NET.Backend.Blueprint.Api.Model.Commands;
using NET.Backend.Blueprint.Api.Model.Queries;
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
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonController_GetAll_Ok()
    {
        var response = await _fixture.SendAsync(HttpMethod.Get, Route, TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsync<IEnumerable<GetPersonResponse>>();
        var expected = await _jsonResourceProvider.CreateObjectByResourceAsync<IEnumerable<GetPersonResponse>>("Get_Person_Response.json");

        result.Should().BeEquivalentTo(expected, options => options
            .Excluding(x => x.Id)
            .Excluding(x => x.Version)
            .Excluding(x => x.Created)
            .Excluding(x => x.CreatedBy)
            .For(x => x.Addresses).Exclude(x => x.Id)
            .For(x => x.Addresses).Exclude(x => x.Version)
            .For(x => x.Addresses).Exclude(x => x.Created)
            .For(x => x.Addresses).Exclude(x => x.CreatedBy)
        );
    }
}
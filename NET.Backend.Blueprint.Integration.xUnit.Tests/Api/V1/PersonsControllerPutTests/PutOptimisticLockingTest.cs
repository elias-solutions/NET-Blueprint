using System.Net;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions;
using Microsoft.AspNetCore.Mvc;
using NET.Backend.Blueprint.Application.Model.Queries;
using NET.Backend.Blueprint.Extensions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Extensions;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPutTests;

[Collection(nameof(SharedTestCollection))]
public class PutOptimisticLockingTest(Fixture fixture) : IAsyncLifetime
{
    private const string Route = "/api/v1/persons";
    private readonly EmbeddedJsonResourceProvider _jsonResourceProvider = new(typeof(PutOptimisticLockingTest).Namespace!);

    public async Task InitializeAsync() => await fixture.ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;


    [Fact]
    public async Task PersonsController_Ok()
    {
        var content = await _jsonResourceProvider.CreateHttpContentByResourceAsync("Post_Person_Request.json");
        var response = await fixture.SendAsync(HttpMethod.Post, Route, content, TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var getPersonResponse = await response.Content.ReadAsync<GetPersonResponse>();

        var expectedPerson = await _jsonResourceProvider.CreateObjectByResourceAsync<GetPersonResponse>("Put_Person_Request.json") with { Id = getPersonResponse!.Id, Version = Guid.NewGuid() };
        response = await fixture.SendAsync(HttpMethod.Put, $"{Route}/{getPersonResponse!.Id}", expectedPerson.ToJson().ToStringContent(), TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var problemDetails = await response.Content.ReadAsync<ProblemDetails>();
        problemDetails.Should().BeEquivalentTo(new ProblemDetails
        {
            Status = (int)HttpStatusCode.BadRequest,
            Title = "BadRequest - Entity version conflict",
            Detail = "Entity has been updated through other user."
        });
    }

    [Fact]
    public async Task PersonsController_StaleVersionAfterUpdate_BadRequest()
    {
        var content = await _jsonResourceProvider.CreateHttpContentByResourceAsync("Post_Person_Request.json");
        var response = await fixture.SendAsync(HttpMethod.Post, Route, content, TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var createdPerson = await response.Content.ReadAsync<GetPersonResponse>();

        response = await fixture.SendAsync(HttpMethod.Get, $"{Route}/{createdPerson!.Id}", null, TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var person = await response.Content.ReadAsync<GetPersonResponse>();

        var updateRequest = await _jsonResourceProvider.CreateObjectByResourceAsync<GetPersonResponse>("Put_Person_Request.json") with { Id = person!.Id, Version = person.Version };
        response = await fixture.SendAsync(HttpMethod.Put, $"{Route}/{person.Id}", updateRequest.ToJson().ToStringContent(), TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        response = await fixture.SendAsync(HttpMethod.Put, $"{Route}/{person.Id}", updateRequest.ToJson().ToStringContent(), TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
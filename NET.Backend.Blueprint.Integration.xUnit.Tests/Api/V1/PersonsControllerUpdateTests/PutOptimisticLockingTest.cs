using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Extensions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerUpdateTests;

[Collection(nameof(SharedTestCollection))]
public class PutOptimisticLockingTest : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    private PersonDto? _dbPerson;
    private const string Route = "/api/v1/persons";

    public PutOptimisticLockingTest(IntegrationTestFixture fixture)
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
        var expectedPerson = await _fixture.CreateExpected<PersonDto>("Put_Person_Request.json") with { Id = _dbPerson!.Id, Version = Guid.NewGuid() };

        var response = await _fixture.PutAsync(TestUsers.Admin, $"{Route}/{_dbPerson!.Id}", expectedPerson);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var problemDetails = await response.Content.ReadAsync<ProblemDetails>();
        problemDetails.Should().BeEquivalentTo(new ProblemDetails
        {
            Status = (int)HttpStatusCode.BadRequest,
            Title = "Entity version conflict",
            Detail = "Entity has been updated through other user."
        });
    }
}
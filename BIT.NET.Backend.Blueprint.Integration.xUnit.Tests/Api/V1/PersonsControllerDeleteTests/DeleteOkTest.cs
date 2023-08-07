using System.Net;
using BIT.NET.Backend.Blueprint.Extensions;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerDeleteTests;

[Collection(nameof(SharedTestCollection))]
public class DeleteOkTest : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    private PersonDto _dbPerson = default!;
    private const string Route = "/api/v1/persons";

    public DeleteOkTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        var response = await _fixture.PostAsync(TestUsers.Admin, Route, "Post_Person_Request.json");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        _dbPerson = await response.Content.ReadAsync<PersonDto>();
    } 

    public async Task DisposeAsync() => await _fixture.RespawnerHelper.ResetRespawner();

    [Fact]
    public async Task PersonsController_Delete_Ok()
    {
        var response = await _fixture.DeleteAsync(TestUsers.Admin, $"{Route}/{_dbPerson.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
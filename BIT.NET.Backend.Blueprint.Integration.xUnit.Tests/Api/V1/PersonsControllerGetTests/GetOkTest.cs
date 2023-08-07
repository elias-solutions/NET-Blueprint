using BIT.NET.Backend.Blueprint.Extensions;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using System.Net;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetTests;

[Collection(nameof(SharedTestCollection))]
public class GetOkTest : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    private PersonDto _dbPerson = default!;
    private const string Route = "/api/v1/persons";

    public GetOkTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    public async Task InitializeAsync()
    {
        var response = await _fixture.PostAsync(TestUsers.Admin, Route, "Post_Person_Request.json");
        _dbPerson = await response.Content.ReadAsync<PersonDto>(); 
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public async Task DisposeAsync() => await _fixture.RespawnerHelper.ResetRespawner();

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
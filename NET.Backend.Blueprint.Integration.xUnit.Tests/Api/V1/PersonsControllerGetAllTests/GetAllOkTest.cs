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
    private readonly IntegrationTestFixture _fixture;
    private PersonDto _dbPerson = default!;
    private const string Route = "/api/v1/persons";

    public GetAllOkTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }
    public async Task InitializeAsync()
    {
        var response = await _fixture.PostAsync(TestUsers.Admin, Route, "Post_Person_Request.json");
        _dbPerson = await response.Content.ReadAsync<PersonDto>(); 
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    public async Task DisposeAsync() => await _fixture.RespawnerHelper.ResetAsync();

    [Fact]
    public async Task PersonController_GetAll_Ok()
    {
        var response = await _fixture.GetAsync(TestUsers.Admin, Route);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsync<IEnumerable<PersonDto>>();
        result.Should().BeEquivalentTo(new[] { _dbPerson }, options => options
            .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(1000)))
            .WhenTypeIs<DateTimeOffset>());
    }
}
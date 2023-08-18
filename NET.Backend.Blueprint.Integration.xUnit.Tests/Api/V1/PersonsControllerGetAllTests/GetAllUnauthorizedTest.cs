using System.Net;
using FluentAssertions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetAllTests;

[Collection(nameof(SharedTestCollection))]
public class GetAllUnauthorizedTest
{
    private readonly IntegrationTestFixture _fixture;
    private const string Route = "/api/v1/persons";

    public GetAllUnauthorizedTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task PersonController_GetAll_Unauthorized()
    {
        var response = await _fixture.GetAsync(null, Route);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
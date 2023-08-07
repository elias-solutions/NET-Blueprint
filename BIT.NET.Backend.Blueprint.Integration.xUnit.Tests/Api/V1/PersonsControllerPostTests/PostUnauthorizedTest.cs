using System.Net;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using FluentAssertions;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPostTests;

[Collection(nameof(SharedTestCollection))]
public class PostUnauthorizedTest
{
    private readonly IntegrationTestFixture _fixture;
    private const string Route = "/api/v1/persons";

    public PostUnauthorizedTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task PersonController_Post_Unauthorized()
    {
        var response = await _fixture.PostAsync(null, Route);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
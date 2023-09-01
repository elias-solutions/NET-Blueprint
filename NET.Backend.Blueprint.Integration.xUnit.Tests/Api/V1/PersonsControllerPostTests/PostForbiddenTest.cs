using System.Net;
using FluentAssertions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using NSubstitute;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPostTests;

[Collection(nameof(SharedTestCollection))]
public class PostForbiddenTest
{
    private readonly IntegrationTestFixture _fixture;
    private const string Route = "/api/v1/persons";

    public PostForbiddenTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.UserService.GetCurrentUser().Returns(TestUsers.Standard);
    }

    [Fact]
    public async Task PersonController_Post_Forbidden()
    {
        var response = await _fixture.SendAsync(TestUsers.Standard, Route, HttpMethod.Post);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
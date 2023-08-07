using System.Net;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPostTests;

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
        var response = await _fixture.PostAsync(TestUsers.Standard, Route);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
using System.Net;
using NET.Backend.Blueprint.Integration.xUnit.Tests.SQlite.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.SQlite.Api.V1.PersonsControllerGetAllTests;

[Collection(nameof(SharedTestCollection))]
public class GetAllForbiddenTest
{
    private readonly IntegrationTestFixture _fixture;
    private const string Route = "/api/v1/persons";

    public GetAllForbiddenTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.UserService.GetCurrentUser().Returns(TestUsers.Standard);
    }
    
    [Fact]
    public async Task PersonController_GetAll_Forbidden()
    {
        var response = await _fixture.SendAsync(TestUsers.Standard, Route, HttpMethod.Get);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
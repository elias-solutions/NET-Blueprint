using System.Net;
using NET.Backend.Blueprint.Integration.xUnit.Tests.SQlite.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.SQlite.Api.V1.PersonsControllerDeleteTests;

[Collection(nameof(SharedTestCollection))]
public class DeleteForbiddenTest
{
    private readonly IntegrationTestFixture _fixture;
    private const string Route = "/api/v1/persons";

    public DeleteForbiddenTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task PersonController_Delete_Forbidden()
    {
       var response = await _fixture.SendAsync(TestUsers.Standard, $"{Route}/{Guid.NewGuid()}", HttpMethod.Delete);
       response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
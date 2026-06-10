using System.Net;
using FluentAssertions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerDeleteTests;

[Collection(nameof(SharedTestCollection))]
public class DeleteForbiddenTest(Fixture fixture)
{
    private const string Route = "/api/v1/persons";

    [Fact]
    public async Task PersonController_Delete_Forbidden()
    {
       var response = await fixture.SendAsync(HttpMethod.Delete, $"{Route}/{Guid.NewGuid()}", null, TestUsers.Standard);
       response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
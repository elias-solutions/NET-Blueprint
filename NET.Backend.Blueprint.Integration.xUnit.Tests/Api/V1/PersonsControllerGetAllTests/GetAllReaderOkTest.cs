using System.Net;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetAllTests;

[Collection(nameof(SharedTestCollection))]
public class GetAllReaderOkTest(Fixture fixture)
{
    private const string Route = "/api/v1/persons";

    [Fact]
    public async Task PersonController_GetAll_Reader_Ok()
    {
        var response = await fixture.SendAsync(HttpMethod.Get, Route, null, TestUsers.Reader);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}

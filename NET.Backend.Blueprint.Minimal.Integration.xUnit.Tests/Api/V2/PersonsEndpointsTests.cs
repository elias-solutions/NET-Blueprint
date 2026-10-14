using System.Net;
using NET.Backend.Blueprint.Minimal.Integration.xUnit.Tests.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Minimal.Integration.xUnit.Tests.Api.V2;

[Collection(nameof(SharedTestCollection))]
public class PersonsEndpointsTests(Fixture fixture)
{
    [Fact]
    public async Task GetAll_V2_Ok()
    {
        var response = await fixture.SendAsync(HttpMethod.Get, "/api/v2/persons", TestUsers.Admin);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Jonas Elias", await response.ReadAsync<string>());
    }

    [Fact]
    public async Task GetAll_UnsupportedVersion_NotFound()
    {
        var response = await fixture.SendAsync(HttpMethod.Get, "/api/v9/persons", TestUsers.Admin);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}

using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using FluentAssertions;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetAllTests;

[Collection(nameof(SharedTestCollection))]
public class GetAllUnauthorizedTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";

    public GetAllUnauthorizedTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
    }

    protected override Task InitAsync() => Task.CompletedTask;

    protected override Task DeInitAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonController_GetAll_Unauthorized()
    {
        var response = await GetAsync(null, Route);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
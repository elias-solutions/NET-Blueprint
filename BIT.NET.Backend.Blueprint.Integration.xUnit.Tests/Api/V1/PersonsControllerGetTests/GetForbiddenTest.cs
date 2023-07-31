using System.Net;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetTests;

[Collection(nameof(SharedTestCollection))]
public class GetForbiddenTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";

    public GetForbiddenTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
        UserService.GetCurrentUser().Returns(TestUsers.Standard);
    }

    protected override Task InitAsync() => Task.CompletedTask;

    protected override Task DeInitAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonController_Get_Forbidden()
    {
        var response = await GetAsync(TestUsers.Standard, $"{Route}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
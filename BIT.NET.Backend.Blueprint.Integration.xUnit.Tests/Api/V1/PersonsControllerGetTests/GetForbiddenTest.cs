using System.Net;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetTests;

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
        await AssertGetHttpStatusCodeAsync(TestUsers.Standard, $"{Route}/{Guid.NewGuid()}", HttpStatusCode.Forbidden);
    }
}
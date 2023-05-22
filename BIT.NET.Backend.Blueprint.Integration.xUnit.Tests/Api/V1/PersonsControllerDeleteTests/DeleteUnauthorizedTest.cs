using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerDeleteTests;

public class DeleteUnauthorizedTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";

    public DeleteUnauthorizedTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
    }

    protected override Task InitAsync() => Task.CompletedTask;

    protected override Task DeInitAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonController_Delete_Unauthorized()
    {
        await AssertDeleteUnauthorizedAsync($"{Route}/{Guid.NewGuid()}");
    }
}
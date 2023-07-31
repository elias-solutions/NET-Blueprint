using System.Net;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerDeleteTests;

[Collection(nameof(SharedTestCollection))]
public class DeleteForbiddenTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";

    public DeleteForbiddenTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
    }

    protected override Task InitAsync() => Task.CompletedTask;

    protected override Task DeInitAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonController_Delete_Forbidden()
    {
       var response = await DeleteAsync(TestUsers.Standard, $"{Route}/{Guid.NewGuid()}");
       response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
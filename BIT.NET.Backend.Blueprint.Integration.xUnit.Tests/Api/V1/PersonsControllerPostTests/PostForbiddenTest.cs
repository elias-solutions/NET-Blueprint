using System.Net;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPostTests;

[Collection(nameof(SharedTestCollection))]
public class PostForbiddenTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";

    public PostForbiddenTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
        UserService.GetCurrentUser().Returns(TestUsers.Standard);
    }

    protected override Task InitAsync() => Task.CompletedTask;

    protected override Task DeInitAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonController_Post_Forbidden()
    {
        var response = await PostAsync(TestUsers.Standard, Route);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
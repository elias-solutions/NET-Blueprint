using System.Net;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using NSubstitute;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetAllTests;

public class GetAllForbiddenTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";

    public GetAllForbiddenTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
        UserService.GetCurrentUser().Returns(TestUsers.Standard);
    }
    
    [Fact]
    public async Task PersonController_GetAll_Forbidden()
    {
        var response = await Client.GetAsync(Route);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
using System.Net;
using BIT.NET.Backend.Blueprint.Extensions;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetAllTests;

public class GetAllOkEmptyElementsTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";

    public GetAllOkEmptyElementsTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
        UserService.GetCurrentUser().Returns(TestUsers.Admin);
    }

    [Fact]
    public async Task PersonController_GetAll_Ok()
    {
        var response = await Client.GetAsync(Route);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var personsResult = await response.Content.ReadAsync<IEnumerable<PersonDto>>();
        personsResult.Should().BeEmpty();
    }
}
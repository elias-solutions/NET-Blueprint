using System.Net;
using System.Text;
using System.Text.Json;
using BIT.NET.Backend.Blueprint.Extensions;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetAllTests;

[Collection("Test collection")]
public class GetAllOkTest : IntegrationTestBase
{
    private readonly CreatePersonRequest _request;
    private const string Route = "/api/v1/persons";

    public GetAllOkTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
        _request = new CreatePersonRequest("Jonas", "Elias", new DateTimeOffset(1987, 5, 13, 0, 0, 0, 0, TimeSpan.Zero));
        UserService.GetCurrentUser().Returns(TestUsers.Admin);
    }

    [Fact]
    public async Task PersonController_GetAll_Ok()
    {
        var httpContent = new StringContent(JsonSerializer.Serialize(_request), Encoding.UTF8, "application/json");
        var response = await Client.PostAsync(Route, httpContent);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var addedPerson = await response.Content.ReadAsync<PersonDto>();

        response = await Client.GetAsync(Route);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var personsResult = await response.Content.ReadAsync<IEnumerable<PersonDto>>();
        personsResult.Should().BeEquivalentTo(new[] { addedPerson });
    }
}
using BIT.NET.Backend.Blueprint.Extensions;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerDeleteTests;

[Collection("Test collection")]
public class DeleteOkTest : IntegrationTestBase
{
    private readonly CreatePersonRequest _request;
    private const string Route = "/api/v1/persons";

    public DeleteOkTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
        _request = new CreatePersonRequest("Jonas", "Elias", DateTimeOffset.UtcNow);
        UserService.GetCurrentUser().Returns(TestUsers.Admin);
    }

    [Fact]
    public async Task PersonsController_Delete_Ok()
    {
        var httpContent = new StringContent(JsonSerializer.Serialize(_request), Encoding.UTF8, "application/json");
        var response = await Client.PostAsync(Route, httpContent);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var addedPerson = await response.Content.ReadAsync<PersonDto>();

        response = await Client.DeleteAsync($"{Route}/{addedPerson.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
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

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPostTests;

[Collection("Test collection")]
public class PostOkTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";
    private readonly CreatePersonRequest _request;

    public PostOkTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
        UserService.GetCurrentUser().Returns(TestUsers.Admin);
        _request = new CreatePersonRequest("Jonas", "Elias", DateTimeOffset.UtcNow);
    }

    [Fact]
    public async Task PersonsController_Ok()
    {
        var httpContent = new StringContent(JsonSerializer.Serialize(_request), Encoding.UTF8, "application/json");
        var response = await Client.PostAsync(Route, httpContent);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var addedPerson = await response.Content.ReadAsync<PersonDto>();

        addedPerson.Id.Should().NotBe(Guid.Empty);
        addedPerson.FirstName.Should().Be(_request.FirstName);
        addedPerson.LastName.Should().Be(_request.LastName);
        addedPerson.Birthday.Should().Be(_request.Birthday);
        addedPerson.Created.Should().NotBe(DateTimeOffset.MinValue);
        addedPerson.CreatedBy.Should().NotBe(Guid.Empty);
        addedPerson.Modified.Should().Be(DateTimeOffset.MinValue);
        addedPerson.ModifiedBy.Should().Be(Guid.Empty);
    }
}

using System.Net;
using System.Net.Http.Json;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetAllTests;

[Collection(nameof(SharedTestCollection))]
public class GetAllOkEmptyElementsTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";

    public GetAllOkEmptyElementsTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
    }

    protected override Task InitAsync() => Task.CompletedTask;

    protected override Task DeInitAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonController_GetAll_Ok()
    {
        var response = await GetAsync(TestUsers.Admin, Route);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<IEnumerable<PersonDto>>();
        result.Should().BeEmpty();
    }
}
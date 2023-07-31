using BIT.NET.Backend.Blueprint.Extensions;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetAllTests;

[Collection(nameof(SharedTestCollection))]
public class GetAllOkTest : IntegrationTestBase
{
    private PersonDto _dbPerson = default!;
    private const string Route = "/api/v1/persons";

    public GetAllOkTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
    }

    protected override async Task InitAsync()
    {
        var birthday = DateTime.UtcNow.ToUtcDateTimeOffset();
        var addressRequest = new CreateAddressRequest("Kirchweg", "7A", "Hägendorf", "4641");
        var request = new CreatePersonRequest("Jonas", "Elias", birthday, new[] { addressRequest });
        var response = await PostAsync(TestUsers.Admin, Route, request);
        _dbPerson = await response.Content.ReadAsync<PersonDto>();
    }

    protected override Task DeInitAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonController_GetAll_Ok()
    {
        var response = await GetAsync(TestUsers.Admin, Route);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsync<IEnumerable<PersonDto>>();
        result.Should().BeEquivalentTo(new[] { _dbPerson });
    }
}
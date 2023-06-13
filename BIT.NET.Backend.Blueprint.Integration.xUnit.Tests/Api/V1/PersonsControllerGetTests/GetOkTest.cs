using BIT.NET.Backend.Blueprint.Extensions;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetTests;

[Collection("Test collection")]
public class GetOkTest : IntegrationTestBase
{
    private PersonDto _dbPerson = default!;
    private const string Route = "/api/v1/persons";

    public GetOkTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
    }

    protected override async Task InitAsync()
    {
        var birthday = DateTime.UtcNow.ToUtcDateTimeOffset();
        var addressRequest = new CreateAddressRequest("Kirchweg", "7A", "Hägendorf", "4641");
        var request = new CreatePersonRequest("Jonas", "Elias", birthday, new[] { addressRequest });
        _dbPerson = await AssertPostAsync<PersonDto>(TestUsers.Admin, Route, request);
    } 

    protected override Task DeInitAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonController_Get_Ok()
    {
        var result = await AssertGetAsync<PersonDto>(TestUsers.Admin, $"{Route}/{_dbPerson.Id}");
        result.Should().BeEquivalentTo(_dbPerson);
    }
}
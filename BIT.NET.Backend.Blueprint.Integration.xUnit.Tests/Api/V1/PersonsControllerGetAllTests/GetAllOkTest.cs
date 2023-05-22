using BIT.NET.Backend.Blueprint.Extensions;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetAllTests;

[Collection("Test collection")]
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
        var request = new CreatePersonRequest("Jonas", "Elias", birthday);
        _dbPerson = await AssertPostAsync<PersonDto>(TestUsers.Admin, Route, request);
    }

    protected override Task DeInitAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonController_GetAll_Ok()
    {
        var result = await AssertGetAsync<IEnumerable<PersonDto>>(TestUsers.Admin, Route);
        result.Should().BeEquivalentTo(new[] { _dbPerson });
    }
}
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerDeleteTests;

[Collection("Test collection")]
public class DeleteOkTest : IntegrationTestBase
{
    private PersonDto _dbPerson = default!;
    private const string Route = "/api/v1/persons";

    public DeleteOkTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
    }

    protected override async Task InitAsync()
    {
        var birthday = DateTime.UtcNow.ToDateTimeOffset();
        var request = new CreatePersonRequest("Jonas", "Elias", birthday);
        _dbPerson = await AssertPostAsync<PersonDto>(TestUsers.Admin, Route, request);
    } 

    protected override Task DeInitAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonsController_Delete_Ok()
    {
        await AssertDeleteAsync(TestUsers.Admin, $"{Route}/{_dbPerson.Id}");
    }
}
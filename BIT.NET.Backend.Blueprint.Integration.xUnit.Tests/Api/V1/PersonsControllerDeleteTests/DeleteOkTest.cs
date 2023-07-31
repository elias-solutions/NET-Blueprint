using System.Net;
using BIT.NET.Backend.Blueprint.Extensions;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerDeleteTests;

[Collection(nameof(SharedTestCollection))]
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
        var addressRequest = new CreateAddressRequest("Kirchweg", "7A", "Hägendorf", "4641");
        var request = new CreatePersonRequest("Jonas", "Elias", birthday, new []{ addressRequest });
        var response = await PostAsync(TestUsers.Admin, Route, request);
        _dbPerson = await response.Content.ReadAsync<PersonDto>();
    } 

    protected override Task DeInitAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonsController_Delete_Ok()
    {
        var response = await DeleteAsync(TestUsers.Admin, $"{Route}/{_dbPerson.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
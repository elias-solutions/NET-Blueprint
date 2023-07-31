using System.Net;
using BIT.NET.Backend.Blueprint.Extensions;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerUpdateTests;

[Collection(nameof(SharedTestCollection))]
public class PutOkTest : IntegrationTestBase
{
    private PersonDto? _dbPerson;
    private const string Route = "/api/v1/persons";

    public PutOkTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
    }

    protected override async Task InitAsync()
    {
        var birthday = DateTime.UtcNow.ToUtcDateTimeOffset();
        var addressRequestCh = new CreateAddressRequest("Street One", "1", "City One", "");
        var request = new CreatePersonRequest("Jonas", "Elias", birthday, new[] { addressRequestCh });
        var response = await PostAsync(TestUsers.Admin, Route, request);
        _dbPerson = await response.Content.ReadAsync<PersonDto>();
    }

    protected override Task DeInitAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonsController_Ok()
    {
        var editedAddress = _dbPerson!.Addresses.First() with { Street = "Street One (Home base)" };
        var updatePerson = _dbPerson with { Addresses = new [] { editedAddress } };

        var response = await PutAsync(TestUsers.Admin, $"{Route}/{_dbPerson.Id}", updatePerson);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedPerson = await response.Content.ReadAsync<PersonDto>();
        updatedPerson.Should().BeEquivalentTo(updatePerson, options => options
            .Excluding(dto => dto.Modified)
            .Excluding(dto => dto.ModifiedBy));
    }
}

using BIT.NET.Backend.Blueprint.Extensions;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPostTests;

[Collection("Test collection")]
public class PostOkTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";

    public PostOkTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
    }

    protected override Task InitAsync() => Task.CompletedTask;

    protected override Task DeInitAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonsController_Ok()
    {
        var birthday = DateTime.UtcNow.ToUtcDateTimeOffset();
        var request = new CreatePersonRequest("Jonas", "Elias", birthday);
        var dbPerson = await AssertPostAsync<PersonDto>(TestUsers.Admin, Route, request);
        
        dbPerson.Id.Should().NotBe(Guid.Empty);
        dbPerson.FirstName.Should().Be(request.FirstName);
        dbPerson.LastName.Should().Be(request.LastName);
        dbPerson.Birthday.Should().Be(request.Birthday);
        dbPerson.Created.Should().NotBe(DateTimeOffset.MinValue);
        dbPerson.CreatedBy.Should().NotBe(Guid.Empty);
        dbPerson.Modified.Should().Be(DateTimeOffset.MinValue);
        dbPerson.ModifiedBy.Should().Be(Guid.Empty);
    }
}

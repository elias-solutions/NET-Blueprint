using System.Net;
using BIT.NET.Backend.Blueprint.Extensions;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Extensions;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPostTests;

[Collection(nameof(SharedTestCollection))]
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
        var response = await PostAsync(TestUsers.Admin, Route, "Post_Person_Request.json");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dbPerson = await response.Content.ReadAsync<PersonDto>();
        
        var expectedPerson =  (await "Post_Person_Request.json".ToJsonStringContentAsync()).ReadFromJson<PersonDto>();
        dbPerson.Should().BeEquivalentTo(expectedPerson, options => options
            .Excluding(entity => entity.Id)
            .Excluding(entity => entity.Created)
            .Excluding(entity => entity.CreatedBy)
            .Excluding(entity => entity.Modified)
            .Excluding(entity => entity.ModifiedBy)
            .For(entity => entity.Addresses).Exclude(entity => entity.Id)
            .For(entity => entity.Addresses).Exclude(entity => entity.Created)
            .For(entity => entity.Addresses).Exclude(entity => entity.CreatedBy)
            .For(entity => entity.Addresses).Exclude(entity => entity.Modified)
            .For(entity => entity.Addresses).Exclude(entity => entity.ModifiedBy));
    }
}

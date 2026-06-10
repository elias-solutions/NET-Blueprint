using FluentAssertions;
using NET.Backend.Blueprint.Api.CQRS.Commands;
using NET.Backend.Blueprint.Api.Model.Commands;
using NET.Backend.Blueprint.Api.Model.Queries;
using NET.Backend.Blueprint.Extensions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Extensions;
using System.Net;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPutTests;

[Collection(nameof(SharedTestCollection))]
public class PutOkTest(Fixture fixture) : IAsyncLifetime
{
    private const string Route = "/api/v1/persons";
    private const string CreatedDate = "2023-10-10T14:30:00+00:00";
    private const string ModifiedDate = "2023-11-10T14:30:00+00:00";
    private readonly EmbeddedJsonResourceProvider _jsonResourceProvider = new(typeof(PutOkTest).Namespace);

    public async Task InitializeAsync() => await fixture.ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonsController_Ok()
    {
        var content = await _jsonResourceProvider.CreateHttpContentByResourceAsync("Post_Person_Request.json");
        var response = await fixture.SendAsync(HttpMethod.Post, Route, content, TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var createPersonResponse = await response.Content.ReadAsync<CreatePersonResponse>();

        response = await fixture.SendAsync(HttpMethod.Get, $"{Route}/{createPersonResponse!.Id}", null, TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var person = await response.Content.ReadAsync<GetPersonResponse>();

        var personRequest = UpdatePerson(person);

        response = await fixture.SendAsync(HttpMethod.Put, $"{Route}/{personRequest!.Id}", personRequest.ToJson().ToStringContent(), TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        response = await fixture.SendAsync(HttpMethod.Get, $"{Route}/{createPersonResponse!.Id}", null, TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedPerson  = await response.Content.ReadAsync<GetPersonResponse>();

        var expectedPerson = await _jsonResourceProvider.CreateObjectByResourceAsync<GetPersonResponse>("Get_Person_Response.json");
        updatedPerson.Should().BeEquivalentTo(expectedPerson, options => options
            .Excluding(x => x.Id)
            .Excluding(x => x.Version)
            .For(x => x.Addresses).Exclude(x => x.Id)
            .For(x => x.Addresses).Exclude(x => x.Version));
        person.Addresses.Select(x => x.Version.Should().NotBe(Guid.Empty));
    }

    private UpdatePersonRequest UpdatePerson(GetPersonResponse person)
    {
        var updatePerson = new UpdatePersonRequest(
            person.Id,
            "Joe", "El",
            new DateOnly(2020, 5, 3),
            UpdateAddresses(person.Addresses.ToArray()),
            TestUsers.Admin.Id,
            DateTimeOffset.Parse(CreatedDate),
            TestUsers.Admin.Id,
            DateTimeOffset.Parse(ModifiedDate),
            person.Version);
        
        return updatePerson;
    }

    private IEnumerable<UpdateAddressRequest> UpdateAddresses(GetAddressResponse[] addresses)
    {
        yield return new UpdateAddressRequest(
            addresses[1].Id,
            "Updated Street",
            "Updated Number",
            "Updated City",
            "Updated Postal Code",
            TestUsers.Admin.Id,
            DateTimeOffset.Parse(CreatedDate),
            TestUsers.Admin.Id,
            DateTimeOffset.Parse(ModifiedDate),
            addresses[1].Version
        );
        
        yield return new UpdateAddressRequest(
            Guid.Empty,
            "Crypton Street",
            "Crypton Number",
            "Crypton City",
            "Crypton Postal Code",
            TestUsers.Admin.Id,
            DateTimeOffset.Parse(CreatedDate),
            TestUsers.Admin.Id,
            DateTimeOffset.Parse(ModifiedDate),
            Guid.Empty);
    }
}
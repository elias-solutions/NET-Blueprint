using System.Net;
using FluentAssertions;
using NET.Backend.Blueprint.Api.Model.Commands;
using NET.Backend.Blueprint.Api.Model.Queries;
using NET.Backend.Blueprint.Extensions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Extensions;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerPutTests;

[Collection(nameof(SharedTestCollection))]
public class PutOkTest : IAsyncLifetime
{
    private const string Route = "/api/v1/persons";
    private readonly IntegrationTestFixture _fixture;
    private readonly EmbeddedJsonResourceProvider _jsonResourceProvider;
    private GetPersonResponse _dbPerson = default;

    public PutOkTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _jsonResourceProvider = new EmbeddedJsonResourceProvider(GetType().Namespace!);
    }

    public async Task InitializeAsync()
    {
        await _fixture.DatabaseResetProvider.ResetAsync();

        var content = await _jsonResourceProvider.CreateHttpContentByResourceAsync("Post_Person_Request.json");
        var response = await _fixture.SendAsync(HttpMethod.Post, Route, content, TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        _dbPerson = await response.Content.ReadAsync<GetPersonResponse>();
    }
    
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonsController_Ok()
    {
        var updateAddresses = UpdateAddressesRequests(_dbPerson.Addresses);
        var updatePerson = new UpdatePersonRequest(_dbPerson.Id, "Bill", "Gates", new DateOnly(1955, 11, 28), updateAddresses, _dbPerson.Version);
        
        var response = await _fixture.SendAsync(HttpMethod.Put, $"{Route}/{_dbPerson!.Id}", updatePerson.ToJson().ToStringContent(), TestUsers.Admin);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var person = await response.Content.ReadAsync<GetPersonRequest>();

        var expectedPerson = await _jsonResourceProvider.CreateObjectByResourceAsync<GetPersonRequest>("Put_Person_Request.json");
        person.Should().BeEquivalentTo(expectedPerson, options => options
            .Excluding(x => x.Id)
            .Excluding(x => x.Modified)
            .Excluding(x => x.ModifiedBy)
            .Excluding(x => x.Version)
            .Excluding(x => x.Created)
            .Excluding(x => x.CreatedBy)
            .For(x => x.Addresses).Exclude(x => x.Id)
            .For(x => x.Addresses).Exclude(x => x.Modified)
            .For(x => x.Addresses).Exclude(x => x.ModifiedBy)
            .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(1000)))
            .WhenTypeIs<DateTimeOffset>());
    }

    private UpdateAddressRequest Map(GetAddressResponse address)
    {
        return new UpdateAddressRequest(address.Id, address.Street, address.Number, address.City, address.PostalCode, address.Version);
    }

    private IEnumerable<UpdateAddressRequest> UpdateAddressesRequests(IEnumerable<GetAddressResponse> addresses)
    {
        var updateAddresses = addresses.Select(Map).ToList();

        var newAddress = updateAddresses.Last() with
        {
            AddressId = Guid.Empty,
            City = "Kawaii",
            Street = "Beauty street",
            Number = "15",
            PostalCode = "4321",
        };

        var updateAddress = updateAddresses.First() with
        {
            AddressId = updateAddresses.First().AddressId,
            City = "Honolulu",
            Street = "Most beauty street",
            Number = "16",
            PostalCode = "4321",
        };

        yield return updateAddress;
        yield return newAddress;
    }
}
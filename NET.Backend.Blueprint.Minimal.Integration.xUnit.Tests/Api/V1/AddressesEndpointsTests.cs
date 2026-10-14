using System.Net;
using NET.Backend.Blueprint.Application.CQRS.Commands;
using NET.Backend.Blueprint.Application.Model.Commands;
using NET.Backend.Blueprint.Application.Model.Queries;
using NET.Backend.Blueprint.Minimal.Integration.xUnit.Tests.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Minimal.Integration.xUnit.Tests.Api.V1;

[Collection(nameof(SharedTestCollection))]
public class AddressesEndpointsTests(Fixture fixture) : IAsyncLifetime
{
    private const string Route = "/api/v1/addresses";

    public Task InitializeAsync() => fixture.ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Put_ExistingAddress_OkWithUpdatedAddress()
    {
        var address = await CreateAddressAsync();
        var request = ToUpdateRequest(address) with { City = "Zurich" };

        var response = await fixture.SendAsync(HttpMethod.Put, $"{Route}/{address.Id}", TestUsers.Admin, request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updated = await response.ReadAsync<GetAddressResponse>();
        Assert.Equal("Zurich", updated.City);
        Assert.NotEqual(address.Version, updated.Version);
    }

    [Fact]
    public async Task Put_RouteIdDiffersFromBodyId_BadRequest()
    {
        var address = await CreateAddressAsync();

        var response = await fixture.SendAsync(HttpMethod.Put, $"{Route}/{Guid.NewGuid()}", TestUsers.Admin, ToUpdateRequest(address));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_WithoutUser_Unauthorized()
    {
        var response = await fixture.SendAsync(HttpMethod.Put, $"{Route}/{Guid.NewGuid()}", null);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Put_WithStandardUser_Forbidden()
    {
        var response = await fixture.SendAsync(HttpMethod.Put, $"{Route}/{Guid.NewGuid()}", TestUsers.Standard);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    private async Task<GetAddressResponse> CreateAddressAsync()
    {
        var response = await fixture.SendAsync(HttpMethod.Post, "/api/v1/persons", TestUsers.Admin, TestData.CreatePersonRequest());
        var id = (await response.ReadAsync<CreatePersonResponse>()).Id;
        response = await fixture.SendAsync(HttpMethod.Get, $"/api/v1/persons/{id}", TestUsers.Admin);
        return (await response.ReadAsync<GetPersonResponse>()).Addresses.First();
    }

    private static UpdateAddressRequest ToUpdateRequest(GetAddressResponse address) => new(
        address.Id, address.Street, address.Number, address.City, address.PostalCode,
        address.CreatedId, address.CreatedDate, TestUsers.Admin.Id, TestData.ModifiedDate, address.Version);
}

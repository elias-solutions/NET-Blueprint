using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model.Queries;

namespace NET.Backend.Blueprint.Api.Extensions;

public static class AddressExtensions
{
    public static GetAddressResponse ToGetAddressResponse(this Address address)
    {
        return new GetAddressResponse(
            address.Id,
            address.Street,
            address.Number,
            address.City,
            address.PostalCode,
            address.CreatedId,
            address.CreatedDate,
            address.ModifiedId,
            address.ModifiedDate,
            address.Version);
    }
}
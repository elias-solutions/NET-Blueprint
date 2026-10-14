using NET.Backend.Blueprint.Domain.Entities;
using NET.Backend.Blueprint.Application.Model.Queries;

namespace NET.Backend.Blueprint.Application.Extensions;

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
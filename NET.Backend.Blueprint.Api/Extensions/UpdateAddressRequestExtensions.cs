using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model.Commands;

namespace NET.Backend.Blueprint.Api.Extensions;

public static class UpdateAddressRequestExtensions
{
    public static Address ToNewAddress(this UpdateAddressRequest request)
    {
        return new Address
        {
            Id = Guid.Empty,
            Street = request.Street,
            Number = request.Number,
            City = request.City,
            PostalCode = request.PostalCode,
        };
    }
}
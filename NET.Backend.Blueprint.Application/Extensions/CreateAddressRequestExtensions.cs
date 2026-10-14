using NET.Backend.Blueprint.Domain.Entities;
using NET.Backend.Blueprint.Application.Model.Commands;

namespace NET.Backend.Blueprint.Application.Extensions;

public static class CreateAddressRequestExtensions
{
    public static Address ToNewAddress(this CreateAddressRequest source)
    {
        return new Address
        {
            Id = Guid.Empty,
            City = source.City, 
            Number = source.Number, 
            PostalCode = source.PostalCode, 
            Street = source.Street,
            CreatedDate = source.CreatedDate,
            CreatedId = source.CreatedId
        };
    }
}
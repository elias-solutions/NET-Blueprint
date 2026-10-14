using NET.Backend.Blueprint.Application.Model.Commands;
using NET.Backend.Blueprint.Domain.Entities;

namespace NET.Backend.Blueprint.Application.Extensions;

public static class UpdateAddressRequestExtensions
{
    public static Address ToNewAddress(this UpdateAddressRequest source)
    {
        return new Address
        {
            Id = Guid.Empty,
            Street = source.Street,
            Number = source.Number,
            City = source.City,
            PostalCode = source.PostalCode,
            CreatedId = source.CreatedId,
            CreatedDate = source.CreatedDate,
            ModifiedId = source.ModifiedId,
            ModifiedDate = source.ModifiedDate,
            Version = source.Version
        };
    }

    public static void ApplyTo(this UpdateAddressRequest source, Address target)
    {
        target.Street = source.Street;
        target.Number = source.Number;
        target.City = source.City;
        target.PostalCode = source.PostalCode;
        target.CreatedId = source.CreatedId;
        target.CreatedDate = source.CreatedDate;
        target.ModifiedId = source.ModifiedId;
        target.ModifiedDate = source.ModifiedDate;
    }
}

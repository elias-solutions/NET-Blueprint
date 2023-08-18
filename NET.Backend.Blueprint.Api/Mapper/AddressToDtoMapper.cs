using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;

namespace NET.Backend.Blueprint.Api.Mapper;

public class AddressToDtoMapper
{
    public AddressDto Map(Address entity)
    {
        return new AddressDto(
            entity.Id,
            entity.Street,
            entity.Number,
            entity.City,
            entity.PostalCode,
            entity.CreatedBy,
            entity.Created,
            entity.ModifiedBy,
            entity.Modified);
    }
}
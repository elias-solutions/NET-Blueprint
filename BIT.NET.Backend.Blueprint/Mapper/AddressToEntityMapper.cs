using BIT.NET.Backend.Blueprint.Entities;
using BIT.NET.Backend.Blueprint.Model;

namespace BIT.NET.Backend.Blueprint.Mapper;

public class AddressToEntityMapper
{
    public Address Map(AddressDto dto)
    {
        return new Address
        {
            Id = dto.Id,
            Street = dto.Street,
            Number = dto.Number,
            City = dto.City,
            PostalCode = dto.PostalCode,
            CreatedBy = dto.CreatedBy,
            Created = dto.Created,
            ModifiedBy = dto.ModifiedBy ?? Guid.Empty,
            Modified = dto.Modified ?? DateTimeOffset.MinValue
        };
    }
}
using BIT.NET.Backend.Blueprint.Entities;
using BIT.NET.Backend.Blueprint.Model;

namespace BIT.NET.Backend.Blueprint.Mapper;

public class PersonToDtoMapper
{
    private readonly AddressToDtoMapper _mapper;

    public PersonToDtoMapper(AddressToDtoMapper mapper)
    {
        _mapper = mapper;
    }

    public PersonDto Map(Person entity)
    {
        var addresses = entity.Addresses.Select(_mapper.Map);
        return new PersonDto(
            entity.Id,
            entity.FirstName,
            entity.LastName,
            entity.Birthday,
            addresses,
            entity.CreatedBy,
            entity.Created,
            entity.ModifiedBy,
            entity.Modified);
    }
}
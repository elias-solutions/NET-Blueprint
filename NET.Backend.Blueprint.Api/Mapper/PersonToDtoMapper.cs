using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;

namespace NET.Backend.Blueprint.Api.Mapper;

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
            entity.Modified,
            entity.Version);
    }
}
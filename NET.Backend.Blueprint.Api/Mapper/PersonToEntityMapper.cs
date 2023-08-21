using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;

namespace NET.Backend.Blueprint.Api.Mapper;

public class PersonToEntityMapper
{
    private readonly AddressToEntityMapper _mapper;

    public PersonToEntityMapper(AddressToEntityMapper mapper)
    {
        _mapper = mapper;
    }

    public Person Map(PersonDto dto)
    {
        var addresses = dto.Addresses.Select(_mapper.Map).ToList();
        return new Person
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Birthday = dto.Birthday,
            Addresses = addresses,
            CreatedBy = dto.CreatedBy,
            Created = dto.Created,
            Version = dto.Version
        };
    }
}
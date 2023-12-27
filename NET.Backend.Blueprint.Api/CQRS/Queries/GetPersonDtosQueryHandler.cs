using MediatR;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Api.Repository;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetPersonDtosQuery : IRequest<IEnumerable<PersonDto>>;

public class GetPersonDtosQueryHandler : IRequestHandler<GetPersonDtosQuery, IEnumerable<PersonDto>>
{
    private readonly Repository<Person> _repository;

    public GetPersonDtosQueryHandler(Repository<Person> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PersonDto>> Handle(GetPersonDtosQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(person => person.Include(x => x.Addresses));
        return entities.Select(MapToPersonDto);
    }

    private PersonDto MapToPersonDto(Person person)
    {
        var addressDto = person.Addresses.Select(MapAddress);
        return new PersonDto(
            person.Id,
            person.FirstName,
            person.LastName,
            person.Birthday,
            addressDto,
            person.CreatedBy,
            person.Created,
            person.ModifiedBy,
            person.Modified,
            person.Version);
    }

    private AddressDto MapAddress(Address entity)
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
            entity.Modified,
            entity.Version);
    }
}

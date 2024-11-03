using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record PersonToPersonDtoQuery(Person Person) : IRequest<PersonDto>;

public class PersonToPersonDtoQueryHandler(IMediator mediator) : IRequestHandler<PersonToPersonDtoQuery, PersonDto>
{
    public async Task<PersonDto> Handle(PersonToPersonDtoQuery query, CancellationToken cancellationToken)
    {
        var addressesDto = await mediator.Send(new AddressToAddressDtoQuery(query.Person));
        return new PersonDto(
            query.Person.Id,
            query.Person.FirstName,
            query.Person.LastName,
            query.Person.Birthday,
            addressesDto,
            query.Person.CreatedBy,
            query.Person.Created,
            query.Person.ModifiedBy,
            query.Person.Modified,
            query.Person.Version);
    }
}
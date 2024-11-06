using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;

namespace NET.Backend.Blueprint.Api.CQRS.Queries.Mapper;

public record PersonToPersonModelQuery(Person Person) : IRequest<GetPersonResponse>;

public class PersonToPersonModelQueryHandler(IMediator mediator) : IRequestHandler<PersonToPersonModelQuery, GetPersonResponse>
{
    public async Task<GetPersonResponse> Handle(PersonToPersonModelQuery query, CancellationToken cancellationToken)
    {
        var addressesResponse = await mediator.Send(new AddressToAddressModelQuery(query.Person.Addresses));
        return new GetPersonResponse(
            query.Person.Id,
            query.Person.FirstName,
            query.Person.LastName,
            query.Person.Birthday,
            addressesResponse,
            query.Person.CreatedBy,
            query.Person.Created,
            query.Person.ModifiedBy,
            query.Person.Modified,
            query.Person.Version);
    }
}
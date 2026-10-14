using NET.Backend.Blueprint.Application.Exceptions;
using NET.Backend.Blueprint.Application.Extensions;
using NET.Backend.Blueprint.Application.Model.Queries;
using NET.Backend.Blueprint.Application.Persistence;
using NET.Backend.Blueprint.Domain.Entities;

namespace NET.Backend.Blueprint.Application.CQRS.Queries;

public record GetPersonResponseByIdQuery(Guid PersonId) : IQuery<GetPersonResponse>;

public class GetPersonResponseByIdQueryHandler(IRepository<Person> repository) : IQueryHandler<GetPersonResponseByIdQuery, GetPersonResponse>
{
    public async Task<GetPersonResponse> HandleAsync(GetPersonResponseByIdQuery query, CancellationToken cancellationToken)
    {
        var person = await repository.FirstOrDefaultAsync(p => p.Id == query.PersonId, [p => p.Addresses], cancellationToken)
                     ?? throw new EntityNotFoundException("person", query.PersonId);

        return person.ToGetPersonResponse();
    }
}

using NET.Backend.Blueprint.Application.Extensions;
using NET.Backend.Blueprint.Application.Model.Queries;
using NET.Backend.Blueprint.Application.Persistence;
using NET.Backend.Blueprint.Domain.Entities;

namespace NET.Backend.Blueprint.Application.CQRS.Queries;

public record GetPersonsResponseQuery : IQuery<IReadOnlyList<GetPersonResponse>>;

public class GetPersonsResponseQueryHandler(IRepository<Person> repository) : IQueryHandler<GetPersonsResponseQuery, IReadOnlyList<GetPersonResponse>>
{
    public async Task<IReadOnlyList<GetPersonResponse>> HandleAsync(GetPersonsResponseQuery query, CancellationToken cancellationToken)
    {
        var persons = await repository.GetAllAsync([p => p.Addresses], cancellationToken);
        return persons.Select(person => person.ToGetPersonResponse()).ToList();
    }
}

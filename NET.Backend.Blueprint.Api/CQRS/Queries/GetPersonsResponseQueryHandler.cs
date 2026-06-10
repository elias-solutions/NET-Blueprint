using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.DataAccess;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Extensions;
using NET.Backend.Blueprint.Api.Model.Queries;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetPersonsResponseQuery : IQuery<IEnumerable<GetPersonResponse>>;

public class GetPersonsResponseQueryHandler(Repository<Person> repository) : IQueryHandler<GetPersonsResponseQuery, IEnumerable<GetPersonResponse>>
{
    public async Task<IEnumerable<GetPersonResponse>> HandleAsync(GetPersonsResponseQuery response, CancellationToken cancellationToken)
    {
        var persons = await repository.GetAllAsync(person => person.Include(x => x.Addresses));
        return persons.Select(person => person.ToGetPersonResponse());
    }
}

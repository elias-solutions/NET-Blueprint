using MediatR;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.DataAccess;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Extensions;
using NET.Backend.Blueprint.Api.Model.Queries;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetPersonsResponseQuery : IRequest<IEnumerable<GetPersonResponse>>;

public class GetPersonsResponseQueryHandler(Repository<Person> repository) : IRequestHandler<GetPersonsResponseQuery, IEnumerable<GetPersonResponse>>
{
    public async Task<IEnumerable<GetPersonResponse>> Handle(GetPersonsResponseQuery response, CancellationToken cancellationToken)
    {
        var persons = await repository.GetAllAsync(person => person.Include(x => x.Addresses));
        return persons.Select(person => person.ToGetPersonResponse());
    }
}

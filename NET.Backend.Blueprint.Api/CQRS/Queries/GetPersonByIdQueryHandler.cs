using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.ErrorHandling;
using System.Net;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Repository;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetPersonByIdQuery(Guid PersonId) : IRequest<Person>;

public class GetPersonByIdQueryHandler(Repository<Person> repository) : IRequestHandler<GetPersonByIdQuery, Person>
{
    public async Task<Person> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        return await repository.FirstOrDefaultAsync(
                   person => person.Id == request.PersonId, 
                   person => person.Include(x => x.Addresses)) ?? 
               throw new ProblemDetailsException(
                   HttpStatusCode.BadRequest, "No person found", $"No person with id '{request.PersonId}' found.");
    }
}

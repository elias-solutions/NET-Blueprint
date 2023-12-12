using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.ErrorHandling;
using NET.Backend.Blueprint.Api.Repository.Base;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetPersonByIdQuery(Guid PersonId) : IRequest<Person>;

public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, Person>
{
    private readonly Repository<Person> _repository;

    public GetPersonByIdQueryHandler(Repository<Person> repository)
    {
        _repository = repository;
    }

    public async Task<Person> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.FirstOrDefaultAsync(
                   person => person.Id == request.PersonId, 
                   person => person.Include(x => x.Addresses)) ?? 
               throw new ProblemDetailsException(
                   HttpStatusCode.BadRequest, "No person found", $"No person with id '{request.PersonId}' found.");
    }
}

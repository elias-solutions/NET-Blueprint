using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.ErrorHandling;
using System.Net;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Api.Repository;
using NET.Backend.Blueprint.Api.CQRS.Queries.Mapper;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetPersonResponseByIdQuery(Guid PersonId) : IRequest<GetPersonResponse>;

public class GetPersonResponseByIdQueryHandler(Repository<Person> repository, IMediator mediator) 
    : IRequestHandler<GetPersonResponseByIdQuery, GetPersonResponse>
{
    public async Task<GetPersonResponse> Handle(GetPersonResponseByIdQuery request, CancellationToken cancellationToken)
    {
        var person =  await repository.FirstOrDefaultAsync(
                          person => person.Id == request.PersonId, 
                          person => person.Include(x => x.Addresses)) ?? 
                      throw new ProblemDetailsException(
                          HttpStatusCode.BadRequest, "No person found", $"No person with id '{request.PersonId}' found.");

        return await mediator.Send(new PersonToPersonModelQuery(person));
    }
}

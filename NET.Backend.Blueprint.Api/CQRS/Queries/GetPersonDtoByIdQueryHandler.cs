using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.ErrorHandling;
using System.Net;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Api.Repository;
using System;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetPersonDtoByIdQuery(Guid PersonId) : IRequest<PersonDto>;

public class GetPersonDtoByIdQueryHandler(Repository<Person> repository, IMediator mediator) 
    : IRequestHandler<GetPersonDtoByIdQuery, PersonDto>
{
    public async Task<PersonDto> Handle(GetPersonDtoByIdQuery request, CancellationToken cancellationToken)
    {
        var person =  await repository.FirstOrDefaultAsync(
                          person => person.Id == request.PersonId, 
                          person => person.Include(x => x.Addresses)) ?? 
                      throw new ProblemDetailsException(
                          HttpStatusCode.BadRequest, "No person found", $"No person with id '{request.PersonId}' found.");

        return await mediator.Send(new PersonToPersonDtoQuery(person));
    }
}

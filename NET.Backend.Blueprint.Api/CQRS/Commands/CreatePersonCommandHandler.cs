using MediatR;
using NET.Backend.Blueprint.Api.CQRS.Queries;
using NET.Backend.Blueprint.Api.DataAccess;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Extensions;
using NET.Backend.Blueprint.Api.Model.Commands;
using NET.Backend.Blueprint.Api.Model.Queries;

namespace NET.Backend.Blueprint.Api.CQRS.Commands;

public record CreatePersonCommand(CreatePersonRequest Request) : IRequest<GetPersonResponse>;

public class CreatePersonCommandHandler(
    Repository<Person> personRepository,
    IMediator mediator)
    : IRequestHandler<CreatePersonCommand, GetPersonResponse>
{
    public async Task<GetPersonResponse> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Request.ToNewPerson();
        var person = await personRepository.AddAsync(entity);
        await personRepository.SaveChangesAsync();
        return await mediator.Send(new GetPersonResponseByIdQuery(person.Entity.Id), cancellationToken);
    }
}
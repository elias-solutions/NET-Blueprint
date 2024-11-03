using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Repository;
using NET.Backend.Blueprint.Api.SignalR;

namespace NET.Backend.Blueprint.Api.CQRS.Commands;

public record DeletePersonByIdCommand(Guid PersonId) : IRequest;

public class DeletePersonByIdCommandHandler(Repository<Person> repository, StatusChangeHub statusChangeHub)
    : IRequestHandler<DeletePersonByIdCommand>
{
    public async Task Handle(DeletePersonByIdCommand request, CancellationToken cancellationToken)
    {
        await repository.RemoveAsync(request.PersonId);
        await repository.SaveChangesAsync();
        await statusChangeHub.SendMessage(request.PersonId, nameof(Person), "deleted");
    }
}
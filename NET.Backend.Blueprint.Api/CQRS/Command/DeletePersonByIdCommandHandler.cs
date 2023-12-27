using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Repository;
using NET.Backend.Blueprint.Api.SignalR;
using System;

namespace NET.Backend.Blueprint.Api.CQRS.Command;

public record DeletePersonByIdCommand(Guid PersonId) : IRequest;

public class DeletePersonByIdCommandHandler : IRequestHandler<DeletePersonByIdCommand>
{
    private readonly Repository<Person> _repository;
    private readonly StatusChangeHub _statusChangeHub;

    public DeletePersonByIdCommandHandler(Repository<Person> repository, StatusChangeHub statusChangeHub)
    {
        _repository = repository;
        _statusChangeHub = statusChangeHub;
    }

    public async Task Handle(DeletePersonByIdCommand request, CancellationToken cancellationToken)
    {
        await _repository.RemoveAsync(request.PersonId);
        await _repository.SaveChangesAsync();
        await _statusChangeHub.SendMessage(request.PersonId, nameof(Person), "deleted");
    }
}
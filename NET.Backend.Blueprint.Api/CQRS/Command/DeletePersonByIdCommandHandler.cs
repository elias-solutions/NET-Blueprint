using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Repository.Base;

namespace NET.Backend.Blueprint.Api.CQRS.Command;

public record DeletePersonByIdCommand(Guid PersonId) : IRequest;

public class DeletePersonByIdCommandHandler : IRequestHandler<DeletePersonByIdCommand>
{
    private readonly Repository<Person> _repository;

    public DeletePersonByIdCommandHandler(Repository<Person> repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeletePersonByIdCommand request, CancellationToken cancellationToken)
    {
        await _repository.RemoveAsync(request.PersonId);
    }
}
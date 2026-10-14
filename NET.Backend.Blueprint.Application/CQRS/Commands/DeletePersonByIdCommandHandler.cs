using NET.Backend.Blueprint.Application.Persistence;
using NET.Backend.Blueprint.Domain.Entities;

namespace NET.Backend.Blueprint.Application.CQRS.Commands;

public record DeletePersonByIdCommand(Guid PersonId) : ICommand;

public class DeletePersonByIdCommandHandler(IRepository<Person> repository) : ICommandHandler<DeletePersonByIdCommand>
{
    public async Task HandleAsync(DeletePersonByIdCommand command, CancellationToken cancellationToken)
    {
        await repository.RemoveAsync(command.PersonId, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}

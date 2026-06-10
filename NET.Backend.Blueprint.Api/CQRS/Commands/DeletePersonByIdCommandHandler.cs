using NET.Backend.Blueprint.Api.DataAccess;
using NET.Backend.Blueprint.Api.Entities;

namespace NET.Backend.Blueprint.Api.CQRS.Commands;

public record DeletePersonByIdCommand(Guid PersonId) : ICommand;

public class DeletePersonByIdCommandHandler(Repository<Person> repository) : ICommandHandler<DeletePersonByIdCommand>
{
    public async Task HandleAsync(DeletePersonByIdCommand request, CancellationToken cancellationToken)
    {
        await repository.RemoveAsync(request.PersonId);
        await repository.SaveChangesAsync();
    }
}
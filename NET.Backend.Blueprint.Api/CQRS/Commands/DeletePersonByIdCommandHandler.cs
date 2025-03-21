using MediatR;
using NET.Backend.Blueprint.Api.DataAccess;
using NET.Backend.Blueprint.Api.Entities;

namespace NET.Backend.Blueprint.Api.CQRS.Commands;

public record DeletePersonByIdCommand(Guid PersonId) : IRequest;

public class DeletePersonByIdCommandHandler(Repository<Person> repository) : IRequestHandler<DeletePersonByIdCommand>
{
    public async Task Handle(DeletePersonByIdCommand request, CancellationToken cancellationToken)
    {
        await repository.RemoveAsync(request.PersonId);
        await repository.SaveChangesAsync();
    }
}
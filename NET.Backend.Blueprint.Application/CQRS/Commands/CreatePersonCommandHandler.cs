using NET.Backend.Blueprint.Application.Extensions;
using NET.Backend.Blueprint.Application.Model.Commands;
using NET.Backend.Blueprint.Application.Persistence;
using NET.Backend.Blueprint.Domain.Entities;

namespace NET.Backend.Blueprint.Application.CQRS.Commands;

public record CreatePersonCommand(CreatePersonRequest Request) : ICommand<CreatePersonResponse>;

public record CreatePersonResponse(Guid Id);

public class CreatePersonCommandHandler(IRepository<Person> repository) : ICommandHandler<CreatePersonCommand, CreatePersonResponse>
{
    public async Task<CreatePersonResponse> HandleAsync(CreatePersonCommand command, CancellationToken cancellationToken)
    {
        var entity = command.Request.ToNewPerson();
        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return new CreatePersonResponse(entity.Id);
    }
}

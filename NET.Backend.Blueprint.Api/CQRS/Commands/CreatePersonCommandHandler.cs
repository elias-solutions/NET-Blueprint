using NET.Backend.Blueprint.Api.DataAccess;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Extensions;
using NET.Backend.Blueprint.Api.Model.Commands;

namespace NET.Backend.Blueprint.Api.CQRS.Commands;

public record CreatePersonCommand(CreatePersonRequest Request) : ICommand<CreatePersonResponse>;

public record CreatePersonResponse(Guid Id);

public class CreatePersonCommandHandler(Repository<Person> personRepository) : ICommandHandler<CreatePersonCommand, CreatePersonResponse>
{
    public async Task<CreatePersonResponse> HandleAsync(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Request.ToNewPerson();
        await personRepository.AddAsync(entity);
        await personRepository.SaveChangesAsync();
        return new CreatePersonResponse(entity.Id);
    }

}

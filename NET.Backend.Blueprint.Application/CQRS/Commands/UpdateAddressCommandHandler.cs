using NET.Backend.Blueprint.Application.Model.Commands;
using NET.Backend.Blueprint.Application.Persistence;
using NET.Backend.Blueprint.Domain.Entities;

namespace NET.Backend.Blueprint.Application.CQRS.Commands;

public record UpdateAddressCommand(UpdateAddressRequest Request) : ICommand;

public class UpdateAddressCommandHandler(IRepository<Address> repository) : ICommandHandler<UpdateAddressCommand>
{
    public async Task HandleAsync(UpdateAddressCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var entity = await repository.GetAsync(request.AddressId, cancellationToken);
        entity.Street = request.Street;
        entity.Number = request.Number;
        entity.City = request.City;
        entity.PostalCode = request.PostalCode;
        entity.ModifiedId = request.ModifiedId;
        entity.ModifiedDate = request.ModifiedDate;
        entity.Version = request.Version;

        await repository.UpdateAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}

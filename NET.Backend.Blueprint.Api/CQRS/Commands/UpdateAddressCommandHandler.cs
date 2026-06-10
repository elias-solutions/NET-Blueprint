using NET.Backend.Blueprint.Api.DataAccess;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model.Commands;

namespace NET.Backend.Blueprint.Api.CQRS.Commands;

public record UpdateAddressCommand(UpdateAddressRequest Request) : ICommand;

public class UpdateAddressCommandHandler(Repository<Address> repository) : ICommandHandler<UpdateAddressCommand>
{
    public async Task HandleAsync(UpdateAddressCommand command, CancellationToken cancellationToken)
    {
        var entity = await repository.GetAsync(command.Request.AddressId);
        entity.City = command.Request.City;
        entity.Number = command.Request.Number;
        entity.PostalCode = command.Request.PostalCode;
        entity.Street = command.Request.Street;
        entity.ModifiedId = command.Request.ModifiedId;
        entity.ModifiedDate = command.Request.ModifiedDate;
        entity.Version = command.Request.Version;

        await repository.UpdateAsync(entity);
        await repository.SaveChangesAsync();
    }
}
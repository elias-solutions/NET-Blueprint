using MediatR;
using NET.Backend.Blueprint.Api.DataAccess;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model.Commands;

namespace NET.Backend.Blueprint.Api.CQRS.Commands;

public record UpdateAddressCommand(UpdateAddressRequest Request) : IRequest;

public class UpdateAddressCommandHandler( Repository<Address> repository) 
    : IRequestHandler<UpdateAddressCommand>
{
    public async Task Handle(UpdateAddressCommand command, CancellationToken cancellationToken)
    {
        var entity = await repository.GetAsync(command.Request.AddressId);
        entity.City = command.Request.City;
        entity.Number = command.Request.Number;
        entity.PostalCode = command.Request.PostalCode;
        entity.Street = command.Request.Street;

        await repository.UpdateAsync(entity);
        await repository.SaveChangesAsync();
    }
}
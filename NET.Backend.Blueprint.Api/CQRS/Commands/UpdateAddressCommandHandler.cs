using MediatR;
using NET.Backend.Blueprint.Api.CQRS.Queries;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Api.Repository;

namespace NET.Backend.Blueprint.Api.CQRS.Commands;

public record UpdateAddressCommand(UpdateAddressRequest Request) : IRequest;

public class UpdateAddressCommandHandler(IMediator mediator, Repository<Address> repository) 
    : IRequestHandler<UpdateAddressCommand>
{
    public async Task Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await mediator.Send(new GetAddressByIdQuery(request.Request.AddressId), cancellationToken);

        address.City = request.Request.City;
        address.Number = request.Request.Number;
        address.PostalCode = request.Request.PostalCode;
        address.Street = request.Request.Street;

        await repository.UpdateAsync(address);
        await repository.SaveChangesAsync();
    }
}
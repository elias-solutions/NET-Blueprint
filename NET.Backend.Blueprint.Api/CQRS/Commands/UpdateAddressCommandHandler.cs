using MediatR;
using NET.Backend.Blueprint.Api.CQRS.Queries;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Api.Repository;

namespace NET.Backend.Blueprint.Api.CQRS.Commands;

public record UpdateAddressCommand(AddressDto AddressDto) : IRequest;

public class UpdateAddressCommandHandler(IMediator mediator, Repository<Address> repository) 
    : IRequestHandler<UpdateAddressCommand>
{
    public async Task Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await mediator.Send(new GetAddressByIdQuery(request.AddressDto.Id), cancellationToken);

        address.City = request.AddressDto.City;
        address.Number = request.AddressDto.Number;
        address.PostalCode = request.AddressDto.PostalCode;
        address.Street = request.AddressDto.Street;

        await repository.UpdateAsync(address);
        await repository.SaveChangesAsync();
    }
}
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
        var dbAddress = await mediator.Send(new GetAddressByIdQuery(request.AddressDto.Id), cancellationToken);

        dbAddress.City = request.AddressDto.City;
        dbAddress.Number = request.AddressDto.Number;
        dbAddress.PostalCode = request.AddressDto.PostalCode;
        dbAddress.Street = request.AddressDto.Street;

        await repository.UpdateAsync(dbAddress);
        await repository.SaveChangesAsync();
    }
}
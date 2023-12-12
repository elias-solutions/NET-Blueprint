using MediatR;
using NET.Backend.Blueprint.Api.CQRS.Queries;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Api.Repository.Base;

namespace NET.Backend.Blueprint.Api.CQRS.Command;

public record UpdateAddressCommand(AddressDto AddressDto) : IRequest;

public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand>
{
    private readonly IMediator _mediator;
    private readonly Repository<Address> _repository;

    public UpdateAddressCommandHandler(IMediator mediator, Repository<Address> repository)
    {
        _mediator = mediator;
        _repository = repository;
    }

    public async Task Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var dbAddress = await _mediator.Send(new GetAddressByIdQuery(request.AddressDto.Id), cancellationToken);

        dbAddress.City = request.AddressDto.City;
        dbAddress.Number = request.AddressDto.Number;
        dbAddress.PostalCode = request.AddressDto.PostalCode;
        dbAddress.Street = request.AddressDto.Street;

        await _repository.UpdateAsync(dbAddress);
    }
}
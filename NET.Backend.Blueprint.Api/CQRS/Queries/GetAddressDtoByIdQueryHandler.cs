using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetAddressDtoByIdQuery(Guid AddressId) : IRequest<AddressDto>;

public class GetAddressDtoByIdQueryHandler(IMediator mediator) : IRequestHandler<GetAddressDtoByIdQuery, AddressDto>
{
    public async Task<AddressDto> Handle(GetAddressDtoByIdQuery request, CancellationToken cancellationToken)
    {
        var address = await mediator.Send(new GetAddressByIdQuery(request.AddressId), cancellationToken);
        return Map(address);
    }

    private AddressDto Map(Address address)
    {
        return new AddressDto(
            address.Id,
            address.Street,
            address.Number,
            address.City,
            address.PostalCode,
            address.CreatedBy,
            address.Created,
            address.ModifiedBy,
            address.Modified,
            address.Version);
    }
}
using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetAddressResponseByIdQuery(Guid AddressId) : IRequest<GetAddressResponse>;

public class GetAddressResponseByIdQueryHandler(IMediator mediator) : IRequestHandler<GetAddressResponseByIdQuery, GetAddressResponse>
{
    public async Task<GetAddressResponse> Handle(GetAddressResponseByIdQuery request, CancellationToken cancellationToken)
    {
        var address = await mediator.Send(new GetAddressByIdQuery(request.AddressId), cancellationToken);
        return Map(address);
    }

    private GetAddressResponse Map(Address address)
    {
        return new GetAddressResponse(
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
using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;

namespace NET.Backend.Blueprint.Api.CQRS.Queries.Mapper;

public record AddressToAddressModelQuery(IEnumerable<Address> Addresses) : IRequest<IEnumerable<GetAddressResponse>>;

public class AddressToAddressModelQueryHandler : IRequestHandler<AddressToAddressModelQuery, IEnumerable<GetAddressResponse>>
{
    public Task<IEnumerable<GetAddressResponse>> Handle(AddressToAddressModelQuery query, CancellationToken cancellationToken)
    {
        var addressesDto = query.Addresses.Select(Map);
        return Task.FromResult(addressesDto);
    }

    private static GetAddressResponse Map(Address address)
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
using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetAddressesDtoByAddressesQuery(IEnumerable<Address> Addresses) : IRequest<IEnumerable<AddressDto>>;

public class GetAddressesDtoByAddressesQueryHandler : IRequestHandler<GetAddressesDtoByAddressesQuery, IEnumerable<AddressDto>>
{
    public Task<IEnumerable<AddressDto>> Handle(GetAddressesDtoByAddressesQuery request, CancellationToken cancellationToken)
    {
        var entities = request.Addresses.Select(Map);
        return Task.FromResult(entities);
    }

    private AddressDto Map(Address entity)
    {
        return new AddressDto(
            entity.Id,
            entity.Street,
            entity.Number,
            entity.City,
            entity.PostalCode,
            entity.CreatedBy,
            entity.Created,
            entity.ModifiedBy,
            entity.Modified,
            entity.Version);
    }
}
using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record AddressToAddressDtoQuery(Person Person) : IRequest<IEnumerable<AddressDto>>;

public class AddressToAddressDtoQueryHandler : IRequestHandler<AddressToAddressDtoQuery, IEnumerable<AddressDto>>
{
    public Task<IEnumerable<AddressDto>> Handle(AddressToAddressDtoQuery query, CancellationToken cancellationToken)
    {
        var addressesDto = query.Person.Addresses.Select(Map);
        return Task.FromResult(addressesDto);
    }

    private static AddressDto Map(Address address)
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
using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetAddressesByAddressesDtoQuery(IEnumerable<AddressDto> AddressesDto) : IRequest<IEnumerable<Address>>;

public class GetAddressesByAddressesDtoQueryHandler : IRequestHandler<GetAddressesByAddressesDtoQuery, IEnumerable<Address>>
{
    public Task<IEnumerable<Address>> Handle(GetAddressesByAddressesDtoQuery request, CancellationToken cancellationToken)
    {
        var result = request.AddressesDto.Select(Map);
        return Task.FromResult(result);
    }

    private Address Map(AddressDto dto)
    {
        return new Address
        {
            City = dto.City,
            Number = dto.Number,
            PostalCode = dto.PostalCode,
            Street = dto.Street,
            Version = dto.Version
        };
    }
}
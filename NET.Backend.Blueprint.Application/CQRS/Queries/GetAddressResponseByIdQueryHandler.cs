using NET.Backend.Blueprint.Application.Extensions;
using NET.Backend.Blueprint.Application.Model.Queries;
using NET.Backend.Blueprint.Application.Persistence;
using NET.Backend.Blueprint.Domain.Entities;

namespace NET.Backend.Blueprint.Application.CQRS.Queries;

public record GetAddressResponseByIdQuery(Guid Id) : IQuery<GetAddressResponse>;

public class GetAddressResponseByIdQueryHandler(IRepository<Address> repository) : IQueryHandler<GetAddressResponseByIdQuery, GetAddressResponse>
{
    public async Task<GetAddressResponse> HandleAsync(GetAddressResponseByIdQuery query, CancellationToken cancellationToken)
    {
        var address = await repository.GetAsync(query.Id, cancellationToken);
        return address.ToGetAddressResponse();
    }
}

using NET.Backend.Blueprint.Api.CQRS.Commands;
using NET.Backend.Blueprint.Api.DataAccess;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Extensions;
using NET.Backend.Blueprint.Api.Model.Queries;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetAddressResponseByIdQuery(Guid Id) : IQuery<GetAddressResponse>;

public class GetAddressResponseByIdQueryHandler(Repository<Address> repository) : IQueryHandler<GetAddressResponseByIdQuery, GetAddressResponse>
{
    public async Task<GetAddressResponse> HandleAsync(GetAddressResponseByIdQuery request, CancellationToken cancellationToken)
    {
        var address = await repository.GetAsync(request.Id);
        return address.ToGetAddressResponse();
    }
}
using MediatR;
using NET.Backend.Blueprint.Api.DataAccess;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Extensions;
using NET.Backend.Blueprint.Api.Model.Queries;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetAddressResponseByIdQuery(Guid Id) : IRequest<GetAddressResponse>;

public class GetAddressResponseByIdQueryHandler(Repository<Address> repository) : IRequestHandler<GetAddressResponseByIdQuery, GetAddressResponse>
{
    public async Task<GetAddressResponse> Handle(GetAddressResponseByIdQuery request, CancellationToken cancellationToken)
    {
        var address = await repository.GetAsync(request.Id);
        return address.ToGetAddressResponse();
    }
}
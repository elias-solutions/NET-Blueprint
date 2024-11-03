using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Repository;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetAddressByIdQuery(Guid Id) : IRequest<Address>;

public class GetAddressByIdQueryHandler(Repository<Address> repository) : IRequestHandler<GetAddressByIdQuery, Address>
{
    public async Task<Address> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetAsync(request.Id);
    }
}
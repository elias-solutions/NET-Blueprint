using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Api.Repository.Base;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetAddressByIdQuery(Guid Id) : IRequest<Address>;

public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, Address>
{
    private readonly Repository<Address> _repository;

    public GetAddressByIdQueryHandler(Repository<Address> repository)
    {
        _repository = repository;
    }

    public async Task<Address> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAsync(request.Id);
    }
}
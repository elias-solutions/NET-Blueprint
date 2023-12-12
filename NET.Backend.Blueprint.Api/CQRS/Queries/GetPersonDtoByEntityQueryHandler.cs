using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetPersonDtoByEntityQuery(Person Person) : IRequest<PersonDto>;

public class GetPersonDtoByEntityQueryHandler : IRequestHandler<GetPersonDtoByEntityQuery, PersonDto>
{
    private readonly IMediator _mediator;

    public GetPersonDtoByEntityQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<PersonDto> Handle(GetPersonDtoByEntityQuery request, CancellationToken cancellationToken)
    {
        var entity = request.Person;
        var addresses = await _mediator.Send(new GetAddressesDtoByAddressesQuery(entity.Addresses), cancellationToken);
        return new PersonDto(
            entity.Id,
            entity.FirstName,
            entity.LastName,
            entity.Birthday,
            addresses,
            entity.CreatedBy,
            entity.Created,
            entity.ModifiedBy,
            entity.Modified,
            entity.Version);
    }
}
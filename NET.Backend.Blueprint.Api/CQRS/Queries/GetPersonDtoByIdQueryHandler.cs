using MediatR;
using NET.Backend.Blueprint.Api.Model;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetPersonDtoByIdQuery(Guid PersonId) : IRequest<PersonDto>;

public class GetPersonDtoByIdQueryHandler : IRequestHandler<GetPersonDtoByIdQuery, PersonDto>
{
    private readonly IMediator _mediator;

    public GetPersonDtoByIdQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<PersonDto> Handle(GetPersonDtoByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _mediator.Send(new GetPersonByIdQuery(request.PersonId), cancellationToken);
        var dto = await _mediator.Send(new GetPersonDtoByEntityQuery(entity), cancellationToken);
        return dto;
    }
}

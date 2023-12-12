using MediatR;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Api.Repository.Base;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetPersonDtosQuery : IRequest<IEnumerable<PersonDto>>;

public class GetPersonDtosQueryHandler : IRequestHandler<GetPersonDtosQuery, IEnumerable<PersonDto>>
{
    private readonly IMediator _mediator;
    private readonly Repository<Person> _repository;

    public GetPersonDtosQueryHandler(
        IMediator mediator,
        Repository<Person> repository)
    {
        _mediator = mediator;
        _repository = repository;
    }

    public async Task<IEnumerable<PersonDto>> Handle(GetPersonDtosQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(person => person.Include(x => x.Addresses));

        var dtoList = new List<PersonDto>();
        foreach (var entity in entities)
        {
            var dto = await _mediator.Send(new GetPersonDtoByEntityQuery(entity), cancellationToken);
            dtoList.Add(dto);
        }

        return dtoList;
    }
}

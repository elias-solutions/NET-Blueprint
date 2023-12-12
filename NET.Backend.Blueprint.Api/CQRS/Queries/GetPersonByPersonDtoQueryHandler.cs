using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;
using System.Net;
using System.Threading;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetPersonByPersonDtoQuery(PersonDto PersonDto) : IRequest<Person>;

public class GetPersonByPersonDtoQueryHandler : IRequestHandler<GetPersonByPersonDtoQuery, Person>
{
    private readonly IMediator _mediator;

    public GetPersonByPersonDtoQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Person> Handle(GetPersonByPersonDtoQuery request, CancellationToken cancellationToken)
    {
        var personDto = request.PersonDto;
        var addresses = await _mediator.Send(new GetAddressesByAddressesDtoQuery(personDto.Addresses), cancellationToken);

        return new Person
        {
            FirstName = personDto.FirstName,
            LastName = personDto.LastName,
            Birthday = personDto.Birthday,
            Version = personDto.Version,
            Addresses = addresses.ToList()
        };
    }
}
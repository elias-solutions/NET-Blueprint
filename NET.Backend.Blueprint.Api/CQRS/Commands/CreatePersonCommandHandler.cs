using MediatR;
using NET.Backend.Blueprint.Api.CQRS.Queries;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Api.Repository;
using NET.Backend.Blueprint.Api.SignalR;

namespace NET.Backend.Blueprint.Api.CQRS.Commands;

public record CreatePersonCommand(CreatePersonRequest CreatePersonRequest) : IRequest<PersonDto>;

public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, PersonDto>
{
    private readonly Repository<Person> _personRepository;
    private readonly Repository<Address> _addressRepository;
    private readonly StatusChangeHub _statusChangeHub;
    private readonly IMediator _mediator;

    public CreatePersonCommandHandler(
        Repository<Person> personRepository, 
        Repository<Address> addressRepository,
        StatusChangeHub statusChangeHub,
        IMediator mediator)
    {
        _personRepository = personRepository;
        _addressRepository = addressRepository;
        _statusChangeHub = statusChangeHub;
        _mediator = mediator;
    }

    public async Task<PersonDto> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var createAddresses = request.CreatePersonRequest.Addresses;
        var createPerson = request.CreatePersonRequest;
        var collection = await CreateAddressesAsync(createAddresses);
        var entity = new Person
        {
            FirstName = createPerson.FirstName,
            LastName = createPerson.LastName,
            Birthday = createPerson.Birthday,
            Addresses = collection.ToList(),
        };

        var person = await _personRepository.AddAsync(entity);
        await _personRepository.SaveChangesAsync();
        await _statusChangeHub.SendMessage(entity.Id, nameof(person), "added");

        return await _mediator.Send(new GetPersonDtoByIdQuery(person.Entity.Id), cancellationToken);
    }

    private async Task<IEnumerable<Address>> CreateAddressesAsync(IEnumerable<CreateAddressRequest> addresses)
    {
        var entities = addresses.Select(x => new Address { City = x.City, Number = x.Number, PostalCode = x.PostalCode, Street = x.Street }).ToList();
        foreach (var address in entities)
        {
            await _addressRepository.AddAsync(address);
        }

        return entities;
    }
}
using MediatR;
using NET.Backend.Blueprint.Api.CQRS.Queries;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Api.Repository;
using NET.Backend.Blueprint.Api.SignalR;

namespace NET.Backend.Blueprint.Api.CQRS.Commands;

public record CreatePersonCommand(CreatePersonRequest CreatePersonRequest) : IRequest<GetPersonResponse>;

public class CreatePersonCommandHandler(
    Repository<Person> personRepository,
    StatusChangeHub statusChangeHub,
    IMediator mediator)
    : IRequestHandler<CreatePersonCommand, GetPersonResponse>
{
    public async Task<GetPersonResponse> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var entity = CreatePerson(request);

        var person = await personRepository.AddAsync(entity);
        await personRepository.SaveChangesAsync();
        await statusChangeHub.SendMessage(entity.Id, nameof(person), "added");
        return await mediator.Send(new GetPersonResponseByIdQuery(person.Entity.Id), cancellationToken);
    }

    private static Person CreatePerson(CreatePersonCommand request)
    {
        var entity = new Person
        {
            FirstName = request.CreatePersonRequest.FirstName,
            LastName = request.CreatePersonRequest.LastName,
            Birthday = request.CreatePersonRequest.Birthday,
            Addresses = request.CreatePersonRequest.Addresses.Select(CreateAddress).ToList()
        };
        return entity;
    }

    private static Address CreateAddress(CreateAddressRequest x)
    {
        return new Address { City = x.City, Number = x.Number, PostalCode = x.PostalCode, Street = x.Street };
    }
}
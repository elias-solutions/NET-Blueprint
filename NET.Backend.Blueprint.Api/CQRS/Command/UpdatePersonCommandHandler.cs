using MediatR;
using NET.Backend.Blueprint.Api.CQRS.Queries;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Api.Repository;

namespace NET.Backend.Blueprint.Api.CQRS.Command;

public record UpdatePersonCommand(PersonDto PersonDto) : IRequest;

public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand>
{
    private readonly Repository<Person> _repository;
    private readonly IMediator _mediator;

    public UpdatePersonCommandHandler(Repository<Person> repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }

    public async Task Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var dbEntity = await _mediator.Send(new GetPersonByIdQuery(request.PersonDto.Id), cancellationToken);
        dbEntity.FirstName = request.PersonDto.FirstName;
        dbEntity.LastName = request.PersonDto.LastName;
        dbEntity.Birthday = request.PersonDto.Birthday;
        dbEntity.Version = request.PersonDto.Version;

        EntitiesToUpdate(request, dbEntity);
        EntitiesToDelete(request, dbEntity);
        EntitiesToAdd(request, dbEntity);

        await _repository.UpdateAsync(dbEntity);
        await _repository.SaveChangesAsync();
    }

    private void EntitiesToUpdate(UpdatePersonCommand request, Person dbEntity)
    {
        var entitiesToUpdate = dbEntity.Addresses.Select(x => x.Id).Intersect(request.PersonDto.Addresses.Select(x => x.Id));
        foreach (var id in entitiesToUpdate)
        {
            var newAddress = request.PersonDto.Addresses.Single(x => x.Id == id);
            var oldAddress = dbEntity.Addresses.Single(x => x.Id == id);
            UpdateAddress(oldAddress, newAddress);
        }
    }

    private static void EntitiesToAdd(UpdatePersonCommand request, Person dbEntity)
    {
        var entitiesToAdd = request.PersonDto.Addresses.Select(x => x.Id).Except(dbEntity.Addresses.Select(x => x.Id));
        foreach (var id in entitiesToAdd)
        {
            var address = request.PersonDto.Addresses.Single(x => x.Id == id);
            var newAddress = new Address 
                { City = address.City, PostalCode = address.PostalCode, Number = address.Number, Street = address.Street };
            dbEntity.Addresses.Add(newAddress);
        }
    }

    private static void EntitiesToDelete(UpdatePersonCommand request, Person dbEntity)
    {
        var entitiesToDelete = dbEntity.Addresses.Select(x => x.Id).Except(request.PersonDto.Addresses.Select(x => x.Id));
        foreach (var id in entitiesToDelete)
        {
            var dbAddress = dbEntity.Addresses.Single(x => x.Id == id);
            dbEntity.Addresses.Remove(dbAddress);
        }
    }

    private void UpdateAddress(Address dbAddressDto, AddressDto address)
    {   
        dbAddressDto.City = address.City;
        dbAddressDto.Number = address.Number;
        dbAddressDto.Street = address.Street;
        dbAddressDto.PostalCode = address.PostalCode;
    }
}
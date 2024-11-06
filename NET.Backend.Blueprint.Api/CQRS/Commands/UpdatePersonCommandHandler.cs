using MediatR;
using NET.Backend.Blueprint.Api.CQRS.Queries;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Api.Repository;
using NET.Backend.Blueprint.Api.SignalR;

namespace NET.Backend.Blueprint.Api.CQRS.Commands;


public record UpdatePersonCommand(UpdatePersonRequest Request) : IRequest;

public class UpdatePersonCommandHandler(
    Repository<Person> repository,
    IMediator mediator,
    StatusChangeHub statusChangeHub)
    : IRequestHandler<UpdatePersonCommand>
{
    public async Task Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var dbEntity = await mediator.Send(new GetPersonByIdQuery(request.Request.Id), cancellationToken);
        dbEntity.FirstName = request.Request.FirstName;
        dbEntity.LastName = request.Request.LastName;
        dbEntity.Birthday = request.Request.Birthday;
        dbEntity.Version = request.Request.Version;

        EntitiesToUpdate(request, dbEntity);
        EntitiesToDelete(request, dbEntity);
        EntitiesToAdd(request, dbEntity);

        await repository.UpdateAsync(dbEntity);
        await repository.SaveChangesAsync();
        await statusChangeHub.SendMessage(dbEntity.Id, nameof(Person), "updated");
    }

    private void EntitiesToUpdate(UpdatePersonCommand request, Person dbEntity)
    {
        var entitiesToUpdate = dbEntity.Addresses.Select(x => x.Id).Intersect(request.Request.Addresses.Select(x => x.AddressId));
        foreach (var id in entitiesToUpdate)
        {
            var newAddress = request.Request.Addresses.Single(x => x.AddressId == id);
            var oldAddress = dbEntity.Addresses.Single(x => x.Id == id);
            UpdateAddress(oldAddress, newAddress);
        }
    }

    private static void EntitiesToAdd(UpdatePersonCommand request, Person dbEntity)
    {
        var entitiesToAdd = request.Request.Addresses.Select(x => x.AddressId).Except(dbEntity.Addresses.Select(x => x.Id));
        foreach (var id in entitiesToAdd)
        {
            var address = request.Request.Addresses.Single(x => x.AddressId == id);
            var newAddress = new Address 
                { City = address.City, PostalCode = address.PostalCode, Number = address.Number, Street = address.Street };
            dbEntity.Addresses.Add(newAddress);
        }
    }

    private static void EntitiesToDelete(UpdatePersonCommand request, Person dbEntity)
    {
        var entitiesToDelete = dbEntity.Addresses.Select(x => x.Id).Except(request.Request.Addresses.Select(x => x.AddressId));
        foreach (var id in entitiesToDelete)
        {
            var dbAddress = dbEntity.Addresses.Single(x => x.Id == id);
            dbEntity.Addresses.Remove(dbAddress);
        }
    }

    private void UpdateAddress(Address address, UpdateAddressRequest addressRequest)
    {   
        address.City = addressRequest.City;
        address.Number = addressRequest.Number;
        address.Street = addressRequest.Street;
        address.PostalCode = addressRequest.PostalCode;
    }
}
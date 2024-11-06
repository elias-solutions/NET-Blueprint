using MediatR;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.ErrorHandling;
using NET.Backend.Blueprint.Api.Extensions;
using NET.Backend.Blueprint.Api.Model.Commands;
using NET.Backend.Blueprint.Api.Repository;
using NET.Backend.Blueprint.Api.SignalR;
using System.Net;

namespace NET.Backend.Blueprint.Api.CQRS.Commands;

public record UpdatePersonCommand(UpdatePersonRequest Request) : IRequest;

public class UpdatePersonCommandHandler(
    Repository<Person> repository,
    StatusChangeHub statusChangeHub)
    : IRequestHandler<UpdatePersonCommand>
{
    public async Task Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await repository.FirstOrDefaultAsync(
                   person => person.Id == request.Request.Id,
                   person => person.Include(x => x.Addresses)) ??
               throw new ProblemDetailsException(
                   HttpStatusCode.BadRequest, "No person found", $"No person with id '{request.Request.Id}' found.");

        person.FirstName = request.Request.FirstName;
        person.LastName = request.Request.LastName;
        person.Birthday = request.Request.Birthday;
        person.Version = request.Request.Version;

        var addressIdsEdit = person.Addresses.Select(x => x.Id).Intersect(request.Request.Addresses.Select(x => x.AddressId));
        var dbAddresses = person.Addresses.Where(x => addressIdsEdit.Contains(x.Id));
        foreach (var dbAddress in dbAddresses)
        {
            var newAddress = request.Request.Addresses.Single(x => x.AddressId == dbAddress.Id);
            dbAddress.City = newAddress.City;
            dbAddress.Number = newAddress.Number;
            dbAddress.Street = newAddress.Street;
            dbAddress.PostalCode = newAddress.PostalCode;
        }

        var addressIdsToDelete = person.Addresses
            .Select(x => x.Id)
            .Except(request.Request.Addresses.Select(x => x.AddressId))
            .ToList();
        person.Addresses
            .Where(x => addressIdsToDelete.Contains(x.Id))
            .ToList()
            .ForEach(x => person.Addresses.Remove(x));

        request.Request.Addresses
            .Where(updateAddress => updateAddress.AddressId == Guid.Empty)
            .Select(updateAddress => updateAddress.ToNewAddress())
            .ToList()
            .ForEach(person.Addresses.Add);

        await repository.UpdateAsync(person);
        await repository.SaveChangesAsync();
        await statusChangeHub.SendMessage(person.Id, nameof(Person), "updated");
    }
}
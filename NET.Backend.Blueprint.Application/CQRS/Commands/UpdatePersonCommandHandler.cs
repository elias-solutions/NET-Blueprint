using NET.Backend.Blueprint.Application.Exceptions;
using NET.Backend.Blueprint.Application.Extensions;
using NET.Backend.Blueprint.Application.Model.Commands;
using NET.Backend.Blueprint.Application.Persistence;
using NET.Backend.Blueprint.Domain.Entities;

namespace NET.Backend.Blueprint.Application.CQRS.Commands;

public record UpdatePersonCommand(UpdatePersonRequest Request) : ICommand;

public class UpdatePersonCommandHandler(IRepository<Person> repository) : ICommandHandler<UpdatePersonCommand>
{
    public async Task HandleAsync(UpdatePersonCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var person = await repository.FirstOrDefaultAsync(p => p.Id == request.Id, [p => p.Addresses], cancellationToken)
                     ?? throw new EntityNotFoundException("person", request.Id);

        person.FirstName = request.FirstName;
        person.LastName = request.LastName;
        person.Birthday = request.Birthday;
        person.CreatedId = request.CreatedId;
        person.CreatedDate = request.CreatedDate;
        person.ModifiedId = request.ModifiedId;
        person.ModifiedDate = request.ModifiedDate;
        person.Version = request.Version;

        SynchronizeAddresses(person, request.Addresses.ToList());

        await repository.UpdateAsync(person, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }

    private static void SynchronizeAddresses(Person person, IReadOnlyCollection<UpdateAddressRequest> requestedAddresses)
    {
        var requestedById = requestedAddresses
            .Where(address => address.AddressId != Guid.Empty)
            .ToDictionary(address => address.AddressId);

        foreach (var existing in person.Addresses.ToList())
        {
            if (requestedById.TryGetValue(existing.Id, out var requested))
            {
                requested.ApplyTo(existing);
            }
            else
            {
                person.Addresses.Remove(existing);
            }
        }

        requestedAddresses
            .Where(address => address.AddressId == Guid.Empty)
            .Select(address => address.ToNewAddress())
            .ToList()
            .ForEach(person.Addresses.Add);
    }
}

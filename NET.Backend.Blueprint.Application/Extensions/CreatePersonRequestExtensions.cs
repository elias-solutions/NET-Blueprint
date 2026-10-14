using NET.Backend.Blueprint.Domain.Entities;
using NET.Backend.Blueprint.Application.Model.Commands;

namespace NET.Backend.Blueprint.Application.Extensions;

public static class CreatePersonRequestExtensions
{
    public static Person ToNewPerson(this CreatePersonRequest source)
    {
        var addresses = source.Addresses.Select(x => x.ToNewAddress()).ToList();
        var person = new Person
        {
            Id = Guid.Empty,
            FirstName = source.FirstName,
            LastName = source.LastName,
            Birthday = source.Birthday,
            CreatedId = source.CreatedId,
            CreatedDate = source.CreatedDate,
            Addresses = addresses
        };
        return person;
    }
}
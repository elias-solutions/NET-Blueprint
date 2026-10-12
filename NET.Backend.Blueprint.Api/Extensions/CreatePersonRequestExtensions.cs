using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model.Commands;

namespace NET.Backend.Blueprint.Api.Extensions;

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
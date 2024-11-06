using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model.Queries;

namespace NET.Backend.Blueprint.Api.Extensions;

public static class PersonExtensions
{
    public static GetPersonResponse ToGetPersonResponse(this Person person)
    {
        return new GetPersonResponse(
            person.Id,
            person.FirstName,
            person.LastName,
            person.Birthday,
            person.Addresses.Select(a => a.ToGetAddressResponse()),
            person.CreatedBy,
            person.Created,
            person.ModifiedBy,
            person.Modified,
            person.Version);
    }
}
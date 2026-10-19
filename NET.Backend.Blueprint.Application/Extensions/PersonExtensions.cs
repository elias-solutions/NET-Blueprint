using NET.Backend.Blueprint.Domain.Entities;
using NET.Backend.Blueprint.Application.Model.Queries;

namespace NET.Backend.Blueprint.Application.Extensions;

public static class PersonExtensions
{
    public static GetPersonResponse ToGetPersonResponse(this Person person)
    {
        return new GetPersonResponse(
            person.Id,
            person.FirstName,
            person.LastName,
            person.Birthday,
            person.Gender,
            person.Description,
            person.Email,
            person.Phone,
            person.Type,
            person.Addresses.Select(a => a.ToGetAddressResponse()),
            person.CreatedId,
            person.CreatedDate,
            person.ModifiedId,
            person.ModifiedDate,
            person.Version);
    }
}
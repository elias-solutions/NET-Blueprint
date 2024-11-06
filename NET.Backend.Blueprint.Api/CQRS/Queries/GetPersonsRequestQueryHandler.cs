using MediatR;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Api.Repository;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetPersonsRequestQuery : IRequest<IEnumerable<GetPersonResponse>>;

public class GetPersonsRequestQueryHandler(Repository<Person> repository) : IRequestHandler<GetPersonsRequestQuery, IEnumerable<GetPersonResponse>>
{
    public async Task<IEnumerable<GetPersonResponse>> Handle(GetPersonsRequestQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(person => person.Include(x => x.Addresses));
        return entities.Select(MapToPersonDto);
    }

    private GetPersonResponse MapToPersonDto(Person person)
    {
        var addressDto = person.Addresses.Select(MapAddress);
        return new GetPersonResponse(
            person.Id,
            person.FirstName,
            person.LastName,
            person.Birthday,
            addressDto,
            person.CreatedBy,
            person.Created,
            person.ModifiedBy,
            person.Modified,
            person.Version);
    }

    private GetAddressResponse MapAddress(Address entity)
    {
        return new GetAddressResponse(
            entity.Id,
            entity.Street,
            entity.Number,
            entity.City,
            entity.PostalCode,
            entity.CreatedBy,
            entity.Created,
            entity.ModifiedBy,
            entity.Modified,
            entity.Version);
    }
}

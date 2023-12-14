using MediatR;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.ErrorHandling;
using NET.Backend.Blueprint.Api.Repository.Base;
using System.Net;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Model;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetPersonDtoByIdQuery(Guid PersonId) : IRequest<PersonDto>;

public class GetPersonDtoByIdQueryHandler : IRequestHandler<GetPersonDtoByIdQuery, PersonDto>
{
    private readonly Repository<Person> _repository;

    public GetPersonDtoByIdQueryHandler(Repository<Person> repository)
    {
        _repository = repository;
    }

    public async Task<PersonDto> Handle(GetPersonDtoByIdQuery request, CancellationToken cancellationToken)
    {
        var person =  await _repository.FirstOrDefaultAsync(
                          person => person.Id == request.PersonId, 
                          person => person.Include(x => x.Addresses)) ?? 
                      throw new ProblemDetailsException(
                          HttpStatusCode.BadRequest, "No person found", $"No person with id '{request.PersonId}' found.");

        var personDto = MapToPersonDto(person);
        return personDto;
    }

    private PersonDto MapToPersonDto(Person person)
    {
        var addressDto = person.Addresses.Select(Map);
        return new PersonDto(
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

    private AddressDto Map(Address entity)
    {
        return new AddressDto(
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

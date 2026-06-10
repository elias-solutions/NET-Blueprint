using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.ErrorHandling;
using System.Net;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Extensions;
using NET.Backend.Blueprint.Api.Model.Queries;
using NET.Backend.Blueprint.Api.DataAccess;

namespace NET.Backend.Blueprint.Api.CQRS.Queries;

public record GetPersonResponseByIdQuery(Guid PersonId) : IQuery<GetPersonResponse>;

public class GetPersonResponseByIdQueryHandler(Repository<Person> repository) : IQueryHandler<GetPersonResponseByIdQuery, GetPersonResponse>
{
    public async Task<GetPersonResponse> HandleAsync(GetPersonResponseByIdQuery request, CancellationToken cancellationToken)
    {
        var person = await repository.FirstOrDefaultAsync(
                          person => person.Id == request.PersonId, 
                          person => person.Include(x => x.Addresses)) ?? 
                      throw new ProblemDetailsException(
                          HttpStatusCode.BadRequest, "No person found", $"No person with id '{request.PersonId}' found.");

        return person.ToGetPersonResponse();
    }
}

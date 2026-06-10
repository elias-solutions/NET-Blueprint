using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET.Backend.Blueprint.Api.Authorization;
using NET.Backend.Blueprint.Api.CQRS.Commands;
using NET.Backend.Blueprint.Api.CQRS.Queries;
using NET.Backend.Blueprint.Api.Model.Commands;
using NET.Backend.Blueprint.Api.Model.Queries;

namespace NET.Backend.Blueprint.Api.Controllers.V1;

[ApiController]
[Authorize(Roles = Roles.Admin)]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/persons")]
public class PersonsController(
    IQueryHandler<GetPersonsResponseQuery, IEnumerable<GetPersonResponse>> getPersonsResponseQueryHandler,
    IQueryHandler<GetPersonResponseByIdQuery, GetPersonResponse> getPersonByIdResponseQueryHandler,
    ICommandHandler<CreatePersonCommand, CreatePersonResponse> createPersonCommandHandler,
    ICommandHandler<UpdatePersonCommand> updatePersonCommandHandler,
    ICommandHandler<DeletePersonByIdCommand> deletePersonByIdCommandHandler) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IEnumerable<GetPersonResponse>> GetPersonsV1Async(CancellationToken cancellationToken)
    {
        var result = await getPersonsResponseQueryHandler.HandleAsync(new GetPersonsResponseQuery(), cancellationToken);
        return result;
    }

    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<GetPersonResponse> GetPersonByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await getPersonByIdResponseQueryHandler.HandleAsync(new GetPersonResponseByIdQuery(id), cancellationToken);
        return result;
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<CreatePersonResponse> CreatePersonAsync([FromBody] CreatePersonRequest request, CancellationToken cancellationToken)
    {
        return await createPersonCommandHandler.HandleAsync(new CreatePersonCommand(request), cancellationToken);
    }

    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task UpdatePersonAsync(Guid id, UpdatePersonRequest request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            BadRequest($"Provided AddressId '{id}' and AddressId '{request.Id}' are not equal.'");
        }

        await updatePersonCommandHandler.HandleAsync(new UpdatePersonCommand(request), cancellationToken);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task DeletePersonByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        await deletePersonByIdCommandHandler.HandleAsync(new DeletePersonByIdCommand(id), cancellationToken);
    }
}
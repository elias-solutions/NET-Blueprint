using System.Net;
using Asp.Versioning;
using MediatR;
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
public class PersonsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IEnumerable<GetPersonResponse>> GetPersonsV1Async()
    {
        var result = await mediator.Send(new GetPersonsResponseQuery());
        return result;
    }

    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<GetPersonResponse> GetPersonByIdAsync(Guid id)
    {
        var result = await mediator.Send(new GetPersonResponseByIdQuery(id));
        return result;
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<GetPersonResponse> CreatePersonAsync([FromBody] CreatePersonRequest request)
    {
        var result = await mediator.Send(new CreatePersonCommand(request));
        return result;
    }

    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<GetPersonResponse> UpdatePersonAsync(Guid id, UpdatePersonRequest request)
    {
        if (id != request.Id)
        {
            BadRequest($"Provided AddressId '{id}' and AddressId '{request.Id}' are not equal.'");
        }

        await mediator.Send(new UpdatePersonCommand(request));
        return await mediator.Send(new GetPersonResponseByIdQuery(id));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task DeletePersonByIdAsync(Guid id)
    {
        await mediator.Send(new DeletePersonByIdCommand(id));
    }
}
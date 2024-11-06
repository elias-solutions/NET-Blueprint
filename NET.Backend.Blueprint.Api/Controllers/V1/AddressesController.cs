using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET.Backend.Blueprint.Api.Authorization;
using NET.Backend.Blueprint.Api.CQRS.Commands;
using NET.Backend.Blueprint.Api.CQRS.Queries;
using NET.Backend.Blueprint.Api.Model;

namespace NET.Backend.Blueprint.Api.Controllers.V1;

[ApiController]
[Authorize(Roles = Roles.Admin)]
[Route("api/v{version:apiVersion}/addresses")]
[ApiVersion("1.0")]
public class AddressesController(IMediator mediator) : ControllerBase
{
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<GetAddressResponse> UpdateAddressAsync(Guid id, UpdateAddressRequest request)
    {
        if (id != request.AddressId)
        {
            BadRequest($"Provided Id '{id}' and AddressId '{request.AddressId}' are not equal.'");
        }

        await mediator.Send(new UpdateAddressCommand(request));
        return await mediator.Send(new GetAddressResponseByIdQuery(request.AddressId));
    }
}
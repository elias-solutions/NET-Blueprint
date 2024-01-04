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
public class AddressesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AddressesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<AddressDto> UpdateAddressAsync(Guid id, AddressDto addressDto)
    {
        if (id != addressDto.Id)
        {
            BadRequest($"Provided Id '{id}' and AddressId '{addressDto.Id}' are not equal.'");
        }

        await _mediator.Send(new UpdateAddressCommand(addressDto));
        return await _mediator.Send(new GetAddressDtoByIdQuery(addressDto.Id));
    }
}
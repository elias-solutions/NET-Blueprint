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
[Route("api/v{version:apiVersion}/addresses")]
[ApiVersion("1.0")]
public class AddressesController(
    ICommandHandler<UpdateAddressCommand> updateCommandHandler,
    IQueryHandler<GetAddressResponseByIdQuery, GetAddressResponse> addressQueryHandler) : ControllerBase
{
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<GetAddressResponse> UpdateAddressAsync(Guid id, UpdateAddressRequest request, CancellationToken cancellation)
    {
        if (id != request.AddressId)
        {
            BadRequest($"Provided AddressId '{id}' and AddressId '{request.AddressId}' are not equal.'");
        }

        await updateCommandHandler.HandleAsync(new UpdateAddressCommand(request), cancellation);
        return await addressQueryHandler.HandleAsync(new GetAddressResponseByIdQuery(request.AddressId), cancellation);
    }
}
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET.Backend.Blueprint.Api.Authorization;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Api.Service;

namespace NET.Backend.Blueprint.Api.Controllers;

[ApiController]
[Authorize(Roles = Roles.Admin)]
[Route("api/v1/addresses")]
public class AddressesController : ControllerBase
{
    private readonly AddressService _service;

    public AddressesController(AddressService service)
    {
        _service = service;
    }

    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<AddressDto> UpdateAddressAsync(Guid id, UpdateAddressRequest request)
    {
        var result = await _service.UpdateAsync(id, request);
        return result;
    }
}
using System.Net;
using BIT.NET.Backend.Blueprint.Authorization;
using BIT.NET.Backend.Blueprint.Model;
using BIT.NET.Backend.Blueprint.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BIT.NET.Backend.Blueprint.Controllers;

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
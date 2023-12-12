using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET.Backend.Blueprint.Api.Authorization;

namespace NET.Backend.Blueprint.Api.Controllers.V2
{
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/persons")]
    public class PersonsController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public Task<string> GetPersonsV1Async() => Task.FromResult("Jonas Elias");
    }
}
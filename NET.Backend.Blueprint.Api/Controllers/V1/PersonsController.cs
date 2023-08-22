using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET.Backend.Blueprint.Api.Authorization;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Api.Service;

namespace NET.Backend.Blueprint.Api.Controllers.V1
{
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/persons")]
    public class PersonsController : ControllerBase
    {
        private readonly PersonService _personService;
        private readonly ILogger<PersonsController> _logger;

        public PersonsController(PersonService personService, ILogger<PersonsController> logger)
        {
            _personService = personService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IEnumerable<PersonDto>> GetPersonsV1Async()
        {
            var result = await _personService.GetPersonsAsync();
            return result;
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<PersonDto> GetPersonByIdAsync(Guid id)
        {
            var result = await _personService.GetPersonByIdAsync(id);
            return result;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<PersonDto> CreatePersonAsync([FromBody] CreatePersonRequest request)
        {
            var result = await _personService.CreatePersonsAsync(request);
            return result;
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<PersonDto> UpdatePersonAsync(Guid id, PersonDto personDto)
        {
            if (id != personDto.Id)
            {
                BadRequest($"Provided Id and Person Id of request object have to be equal '{id}/{personDto.Id}'");
            }

            var result = await _personService.UpdatePersonsAsync(personDto);
            return result;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task DeletePersonByIdAsync(Guid id)
        {
            await _personService.DeletePersonByIdAsync(id);
        }
    }
}
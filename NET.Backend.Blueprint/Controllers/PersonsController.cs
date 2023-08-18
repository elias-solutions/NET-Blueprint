using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET.Backend.Blueprint.Authorization;
using NET.Backend.Blueprint.Model;
using NET.Backend.Blueprint.Service;

namespace NET.Backend.Blueprint.Controllers
{
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    [Route("api/v1/persons")]
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
        public async Task<IEnumerable<PersonDto>> GetPersonsAsync()
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
using BIT.NET.Backend.Blueprint.Model;
using BIT.NET.Backend.Blueprint.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BIT.NET.Backend.Blueprint.Controllers
{
    [ApiController]
    [AllowAnonymous]
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
        public async Task<IEnumerable<PersonDto>> GetPersonsAsync()
        {
            var result = await _personService.GetPersonsAsync();
            return result;
        }

        [HttpGet("{id}")]
        public async Task<PersonDto> GetPersonByIdAsync(Guid id)
        {
            var result = await _personService.GetPersonByIdAsync(id);
            return result;
        }

        [HttpPost]
        public async Task<PersonDto> CreatePersonAsync(CreatePersonRequest request)
        {
            var result = await _personService.CreatePersonsAsync(request);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task DeletePersonByIdAsync(Guid id)
        {
            await _personService.DeletePersonByIdAsync(id);
        }

        [HttpDelete]
        public async Task DeletePersonsAsync()
        {
            await _personService.DeletePersonsAsync();
        }
    }
}
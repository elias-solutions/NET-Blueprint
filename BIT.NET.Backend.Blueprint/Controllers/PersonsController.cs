using BIT.NET.Backend.Blueprint.Authorization;
using BIT.NET.Backend.Blueprint.Model;
using BIT.NET.Backend.Blueprint.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BIT.NET.Backend.Blueprint.Controllers
{
    [ApiController]
    //[Authorize(Roles = Roles.Admin)]
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
        public async Task<IActionResult> GetPersonsAsync()
        {
            var result = await _personService.GetPersonsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonByIdAsync(Guid id)
        {
            var result = await _personService.GetPersonByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonAsync(CreatePersonRequest request)
        {
            var result = await _personService.CreatePersonsAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonByIdAsync(Guid id)
        {
            await _personService.DeletePersonByIdAsync(id);
            return Ok();
        }
    }
}
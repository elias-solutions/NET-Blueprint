using BIT.NET.Backend.Blueprint.Authorization;
using BIT.NET.Backend.Blueprint.Model;
using BIT.NET.Backend.Blueprint.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BIT.NET.Backend.Blueprint.Controllers
{
    [ApiController]
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
        [Authorize(Roles = Roles.Admin)]
        public async Task<IEnumerable<GetPersonResponse>> GetPersonsAsync()
        {
            return await _personService.GetPersonsAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<GetPersonResponse> GetPersonByIdAsync(Guid id)
        {
            return await _personService.GetPersonByIdAsync(id);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<GetPersonResponse> CreatePersonAsync(CreatePersonRequest request)
        {
            var savedModel = await _personService.CreatePersonsAsync(request);
            return savedModel;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task DeletePersonByIdAsync(Guid id)
        {
            await _personService.DeletePersonByIdAsync(id);
        }
    }
}
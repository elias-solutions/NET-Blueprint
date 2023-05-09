using BIT.NET.Backend.Blueprint.Model;
using BIT.NET.Backend.Blueprint.Service;
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
        public async Task<IEnumerable<GetPersonResponse>> GetPersonsAsync()
        {
            return await _personService.GetPersonsAsync();
        }

        [HttpGet("{id}")]
        public async Task<GetPersonResponse> GetPersonByIdAsync(Guid id)
        {
            return await _personService.GetPersonByIdAsync(id);
        }

        [HttpPost]
        public async Task<GetPersonResponse> CreatePersonAsync(CreatePersonRequest request)
        {
            var savedModel = await _personService.CreatePersonsAsync(request);
            return savedModel;
        }

        [HttpDelete("{id}")]
        public async Task DeletePersonByIdAsync(Guid id)
        {
            await _personService.DeletePersonByIdAsync(id);
        }
    }
}
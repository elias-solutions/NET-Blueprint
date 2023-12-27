using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET.Backend.Blueprint.Api.Authorization;
using NET.Backend.Blueprint.Api.CQRS.Command;
using NET.Backend.Blueprint.Api.CQRS.Queries;
using NET.Backend.Blueprint.Api.Model;

namespace NET.Backend.Blueprint.Api.Controllers.V1
{
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/persons")]
    public class PersonsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PersonsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IEnumerable<PersonDto>> GetPersonsV1Async()
        {
            var result = await _mediator.Send(new GetPersonDtosQuery());
            return result;
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<PersonDto> GetPersonByIdAsync(Guid id)
        {
            var result = await _mediator.Send(new GetPersonDtoByIdQuery(id));
            return result;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<PersonDto> CreatePersonAsync([FromBody] CreatePersonRequest request)
        {
            var result = await _mediator.Send(new CreatePersonCommand(request));
            return result;
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<PersonDto> UpdatePersonAsync(Guid id, PersonDto personDto)
        {
            if (id != personDto.Id)
            {
                BadRequest($"Provided Id '{id}' and AddressId '{personDto.Id}' are not equal.'");
            }

            await _mediator.Send(new UpdatePersonCommand(personDto));
            return await _mediator.Send(new GetPersonDtoByIdQuery(id));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task DeletePersonByIdAsync(Guid id)
        {
            await _mediator.Send(new DeletePersonByIdCommand(id));
        }
    }
}
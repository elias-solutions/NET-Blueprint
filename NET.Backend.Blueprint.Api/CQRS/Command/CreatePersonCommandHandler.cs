using MediatR;
using NET.Backend.Blueprint.Api.CQRS.Queries;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Api.Repository.Base;

namespace NET.Backend.Blueprint.Api.CQRS.Command
{
    public record CreatePersonCommand(CreatePersonRequest CreatePersonRequest) : IRequest<PersonDto>;

    public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, PersonDto>
    {
        private readonly Repository<Person> _repository;
        private readonly IMediator _mediator;

        public CreatePersonCommandHandler(Repository<Person> repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<PersonDto> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var createAddresses = request.CreatePersonRequest.Addresses;
            var createPerson = request.CreatePersonRequest;
            var entity = new Person
            {
                FirstName = createPerson.FirstName,
                LastName = createPerson.LastName,
                Birthday = createPerson.Birthday,
                Addresses = createAddresses.Select(x => new Address { City = x.City, Number = x.Number, PostalCode = x.PostalCode, Street = x.Street }).ToList(),
            };

            var person = await _repository.AddAsync(entity);
            return await _mediator.Send(new GetPersonDtoByIdQuery(person.Entity.Id));
        }
    }
}

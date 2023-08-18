using System.Net;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Authorization;
using NET.Backend.Blueprint.Api.Entities;
using NET.Backend.Blueprint.Api.ErrorHandling;
using NET.Backend.Blueprint.Api.Mapper;
using NET.Backend.Blueprint.Api.Model;
using NET.Backend.Blueprint.Api.Repository.Base;

namespace NET.Backend.Blueprint.Api.Service
{
    public class PersonService
    {
        private readonly Repository<Person> _repository;
        private readonly PersonToEntityMapper _personToEntityMapper;
        private readonly PersonToDtoMapper _personToDtoMapper;
        private readonly AddressService _addressService;
        private readonly IUserService _userService;

        public PersonService(
            Repository<Person> repository, 
            PersonToEntityMapper personToEntityMapper,
            PersonToDtoMapper personToDtoMapper,
            AddressService addressService,
            IUserService userService)
        {
            _repository = repository;
            _personToEntityMapper = personToEntityMapper;
            _personToDtoMapper = personToDtoMapper;
            _addressService = addressService;
            _userService = userService;
        }

        public async Task<PersonDto> GetPersonByIdAsync(Guid id)
        {
            var entity = await _repository.GetAsync(
                person => person.Id == id, 
                person => person.Include(x => x.Addresses));

            if (entity == null)
            {
                throw new ProblemDetailsException(HttpStatusCode.BadRequest, "No person found", $"No person with id '{id}' found.");
            }

            var model = _personToDtoMapper.Map(entity);
            return model;
        }

        public async Task<IEnumerable<PersonDto>> GetPersonsAsync()
        {
            var entities = await _repository.GetAllAsync(person => person.Include(x => x.Addresses));
            var models = entities.Select(_personToDtoMapper.Map).ToList();
            return models;
        }

        public async Task<PersonDto> UpdatePersonsAsync(PersonDto personDto)
        {
            var dbPerson = await _repository.GetAsync(
                person => person.Id == personDto.Id,
                person => person.Include(x => x.Addresses));

            if (dbPerson == null)
            {
                throw new BadHttpRequestException($"No person with id '{personDto.Id}' found.");
            }

            var updatePerson = _personToEntityMapper.Map(personDto);
            var updatedEntity = await _repository.UpdateAsync(updatePerson, _userService.GetCurrentUser()!.Id);
            return _personToDtoMapper.Map(updatedEntity);
        }

        public async Task<PersonDto> CreatePersonsAsync(CreatePersonRequest request)
        {
            var addresses = request.Addresses.Select(address => new Address
            {
                City = address.City,
                PostalCode = address.PostalCode,
                Number = address.Number,
                Street = address.Street
            });
            
            var entity = new Person
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Birthday = request.Birthday,
                Addresses = addresses.ToList()
            };

            var entityEntry = await _repository.AddAsync(entity, _userService.GetCurrentUser()!.Id);
            return _personToDtoMapper.Map(entityEntry.Entity);
        }

        public async Task DeletePersonByIdAsync(Guid id)
        {
            var entity = await _repository.GetAsync(x => x.Id == id);
            if (entity == null)
            {
                throw new BadHttpRequestException($"No person found with id '{id}'");
            }

            await _repository.RemoveAsync(entity);
        }
    }
}

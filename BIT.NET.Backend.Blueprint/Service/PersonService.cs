using BIT.NET.Backend.Blueprint.Entities;
using BIT.NET.Backend.Blueprint.Model;
using BIT.NET.Backend.Blueprint.Repository.Base;

namespace BIT.NET.Backend.Blueprint.Service
{
    public class PersonService
    {
        private readonly Repository<Person> _repository;

        public PersonService(Repository<Person> repository)
        {
            _repository = repository;
        }

        public async Task<PersonDto> GetPersonByIdAsync(Guid id)
        {
            var entity = await _repository.GetAsync(x => x.Id == id);
            if (entity == null)
            {
                throw new BadHttpRequestException($"No person found with id '{id}'");
            }

            var model = Map(entity);
            return model;
        }

        public async Task<IEnumerable<PersonDto>> GetPersonsAsync()
        {
            var entities = await _repository.GetAllAsync();
            var models = entities.Select(Map).ToList();
            return models;
        }

        public async Task<PersonDto> CreatePersonsAsync(CreatePersonRequest request)
        {
            var entity = new Person
            {
                FristName = request.FirstName,
                LastName = request.LastName,
                Birthday = request.Birthday
            };

            var entityEntry = await _repository.AddAsync(entity, Guid.NewGuid());
            return Map(entityEntry.Entity);
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

        public async Task DeletePersonsAsync()
        {
            var persons = await _repository.GetAllAsync();
            foreach (var person in persons)
            {
                await _repository.RemoveAsync(person);
            }
        }

        private PersonDto Map(Person entity)
        {
            return new PersonDto(
                entity.Id, 
                entity.FristName, 
                entity.LastName, 
                entity.Birthday, 
                entity.CreatedBy, 
                entity.Created, 
                entity.ModifiedBy, 
                entity.Modified);
        }
    }
}

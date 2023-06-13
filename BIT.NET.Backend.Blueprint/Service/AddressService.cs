using System.Linq;
using System.Runtime.CompilerServices;
using BIT.NET.Backend.Blueprint.Authorization;
using BIT.NET.Backend.Blueprint.DataAccess;
using BIT.NET.Backend.Blueprint.Entities;
using BIT.NET.Backend.Blueprint.Mapper;
using BIT.NET.Backend.Blueprint.Model;
using BIT.NET.Backend.Blueprint.Repository.Base;

namespace BIT.NET.Backend.Blueprint.Service
{
    public class AddressService
    {
        private readonly Repository<Address> _repository;
        private readonly IUserService _userService;
        private readonly AddressToEntityMapper _entityMapper;
        private readonly AddressToDtoMapper _dtoMapper;

        public AddressService(
            Repository<Address> repository,
            IUserService userService,
            AddressToEntityMapper entityMapper,
            AddressToDtoMapper dtoMapper)
        {
            _repository = repository;
            _userService = userService;
            _entityMapper = entityMapper;
            _dtoMapper = dtoMapper;
        }

        public async Task<AddressDto> UpdateAsync(Guid id, UpdateAddressRequest request)
        {
            var dbEntity = await _repository.GetAsync(entity => entity.Id == id);

            if (dbEntity == null)
            {
                throw new BadHttpRequestException($"No Address with id '{id}' found.");
            }

            dbEntity.City = request.City;
            dbEntity.Number = request.Number;
            dbEntity.PostalCode = request.PostalCode;
            dbEntity.Street = request.Street;

            var updatedEntity = await _repository.UpdateAsync(dbEntity, _userService.GetCurrentUser()!.Id);
            return _dtoMapper.Map(updatedEntity);
        }
    }
}

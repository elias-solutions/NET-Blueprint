using MediatR;
using NET.Backend.Blueprint.Api.Model.Queries;

namespace NET.Backend.Blueprint.Api.Model.Commands;

public record CreatePersonRequest(
    string FirstName,
    string LastName,
    DateOnly Birthday,
    CreateAddressRequest[] Addresses) : IRequest<GetPersonRequest>;
using NET.Backend.Blueprint.Domain.Enums;

namespace NET.Backend.Blueprint.Application.Model.Queries;

public record GetPersonResponse
(
    Guid Id,
    string FirstName,
    string LastName,
    DateOnly? Birthday,
    Gender? Gender,
    string? Description,
    string? Email,
    string? Phone,
    PersonType? Type,
    IEnumerable<GetAddressResponse> Addresses,
    Guid CreatedId,
    DateTimeOffset CreatedDate,
    Guid? ModifiedId,
    DateTimeOffset? ModifiedDate,
    Guid Version);
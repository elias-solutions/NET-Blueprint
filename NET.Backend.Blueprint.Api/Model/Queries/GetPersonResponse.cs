namespace NET.Backend.Blueprint.Api.Model.Queries;

public record GetPersonResponse
(
    Guid Id,
    string FirstName,
    string LastName,
    DateOnly Birthday,
    IEnumerable<GetAddressResponse> Addresses,
    Guid CreatedBy,
    DateTimeOffset Created,
    Guid? ModifiedBy,
    DateTimeOffset? Modified,
    Guid Version);
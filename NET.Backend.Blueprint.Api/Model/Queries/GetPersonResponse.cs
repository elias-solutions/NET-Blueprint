namespace NET.Backend.Blueprint.Api.Model.Queries;

public record GetPersonResponse
(
    Guid Id,
    string FirstName,
    string LastName,
    DateOnly Birthday,
    IEnumerable<GetAddressResponse> Addresses,
    Guid CreatedId,
    DateTimeOffset CreatedDate,
    Guid? ModifiedId,
    DateTimeOffset? ModifiedDate,
    Guid Version);
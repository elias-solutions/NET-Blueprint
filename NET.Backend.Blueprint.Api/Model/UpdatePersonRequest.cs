namespace NET.Backend.Blueprint.Api.Model;

public record UpdatePersonRequest
(
    Guid Id,
    string FirstName,
    string LastName,
    DateTimeOffset Birthday,
    IEnumerable<UpdateAddressRequest> Addresses,
    Guid CreatedBy,
    DateTimeOffset Created,
    Guid? ModifiedBy,
    DateTimeOffset? Modified,
    Guid Version);
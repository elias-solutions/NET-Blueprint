namespace NET.Backend.Blueprint.Api.Model.Commands;

public record UpdatePersonRequest
(
    Guid Id,
    string FirstName,
    string LastName,
    DateTimeOffset Birthday,
    IEnumerable<UpdateAddressRequest> Addresses,
    Guid Version);
namespace NET.Backend.Blueprint.Api.Model;

public record GetAddressResponse
(
    Guid Id,
    string Street,
    string Number,
    string City,
    string PostalCode,
    Guid CreatedBy,
    DateTimeOffset Created,
    Guid? ModifiedBy,
    DateTimeOffset? Modified,
    Guid Version);


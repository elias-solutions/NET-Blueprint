namespace NET.Backend.Blueprint.Application.Model.Queries;

public record GetAddressResponse
(
    Guid Id,
    string Street,
    string Number,
    string City,
    string PostalCode,
    Guid CreatedId,
    DateTimeOffset CreatedDate,
    Guid? ModifiedId,
    DateTimeOffset? ModifiedDate,
    Guid Version);
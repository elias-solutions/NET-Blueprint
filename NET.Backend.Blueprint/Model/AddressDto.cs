namespace NET.Backend.Blueprint.Model;

public record AddressDto
(
    Guid Id,
    string Street,
    string Number,
    string City,
    string PostalCode,
    Guid CreatedBy,
    DateTimeOffset Created,
    Guid? ModifiedBy,
    DateTimeOffset? Modified);
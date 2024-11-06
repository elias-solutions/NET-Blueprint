namespace NET.Backend.Blueprint.Api.Model.Commands;

public record UpdateAddressRequest(
    Guid AddressId,
    string Street,
    string Number,
    string City,
    string PostalCode,
    Guid Version);
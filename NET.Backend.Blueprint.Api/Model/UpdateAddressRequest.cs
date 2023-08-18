namespace NET.Backend.Blueprint.Api.Model;

public record UpdateAddressRequest(
    Guid AddressId,
    string Street,
    string Number,
    string City,
    string PostalCode);
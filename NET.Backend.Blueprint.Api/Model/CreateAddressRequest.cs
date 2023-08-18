namespace NET.Backend.Blueprint.Api.Model;

public record CreateAddressRequest(
    string Street,
    string Number,
    string City,
    string PostalCode);
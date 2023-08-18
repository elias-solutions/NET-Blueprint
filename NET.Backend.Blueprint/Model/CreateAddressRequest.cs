namespace NET.Backend.Blueprint.Model;

public record CreateAddressRequest(
    string Street,
    string Number,
    string City,
    string PostalCode);
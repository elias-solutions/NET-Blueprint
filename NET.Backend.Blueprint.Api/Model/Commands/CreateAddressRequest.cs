namespace NET.Backend.Blueprint.Api.Model.Commands;

public record CreateAddressRequest(
    string Street,
    string Number,
    string City,
    string PostalCode);
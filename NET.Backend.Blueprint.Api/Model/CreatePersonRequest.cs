namespace NET.Backend.Blueprint.Api.Model;

public record CreatePersonRequest(
    string FirstName, 
    string LastName, 
    DateTimeOffset Birthday,
    CreateAddressRequest[] Addresses);
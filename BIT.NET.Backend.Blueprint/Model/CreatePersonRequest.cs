namespace BIT.NET.Backend.Blueprint.Model;

public record CreatePersonRequest(
    string FirstName, 
    string LastName, 
    DateTimeOffset Birthday,
    CreateAddressRequest[] Addresses);
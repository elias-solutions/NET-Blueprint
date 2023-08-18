namespace NET.Backend.Blueprint.Model;

public record UpdatePersonRequest(
    Guid PersonId,
    string FirstName,
    string LastName,
    DateTimeOffset Birthday,
    UpdateAddressRequest[] Addresses);
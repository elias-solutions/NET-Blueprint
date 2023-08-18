namespace NET.Backend.Blueprint.Model
{
    public record PersonDto
    (
        Guid Id, 
        string FirstName, 
        string LastName, 
        DateTimeOffset Birthday,
        IEnumerable<AddressDto> Addresses,
        Guid CreatedBy,
        DateTimeOffset Created,
        Guid? ModifiedBy,
        DateTimeOffset? Modified);
}

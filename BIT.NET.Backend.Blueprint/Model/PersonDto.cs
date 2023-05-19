namespace BIT.NET.Backend.Blueprint.Model
{
    public record PersonDto
    (
        Guid Id, 
        string FirstName, 
        string LastName, 
        DateTimeOffset Birthday,
        Guid CreatedBy,
        DateTimeOffset Created,
        Guid? ModifiedBy,
        DateTimeOffset? Modified);

    public record CreatePersonRequest(string FirstName, string LastName, DateTimeOffset Birthday);
}

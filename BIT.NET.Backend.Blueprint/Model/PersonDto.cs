namespace BIT.NET.Backend.Blueprint.Model
{
    public record PersonDto
    (
        Guid Id, 
        string FirstName, 
        string LastName, 
        DateTime Birthday,
        Guid CreatedBy,
        DateTime Created,
        Guid? ModifiedBy,
        DateTime? Modified);

    public record CreatePersonRequest(string FirstName, string LastName, DateTime Birthday);
}

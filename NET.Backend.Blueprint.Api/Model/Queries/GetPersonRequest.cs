namespace NET.Backend.Blueprint.Api.Model.Queries
{
    public record GetPersonRequest
    (
        Guid Id,
        string FirstName,
        string LastName,
        DateTimeOffset Birthday,
        IEnumerable<GetAddressResponse> Addresses,
        Guid CreatedBy,
        DateTimeOffset Created,
        Guid? ModifiedBy,
        DateTimeOffset? Modified,
        Guid Version);
}

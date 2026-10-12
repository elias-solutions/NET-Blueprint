using NET.Backend.Blueprint.Api.Entities.Base;

namespace NET.Backend.Blueprint.Api.Entities;

public class Address : EntityBase
{
    public required string Street { get; set; }
    public required string Number { get; set; }
    public required string City { get; set; }
    public required string PostalCode { get; set; }
    public Guid PersonId { get; set; }
    public Person Person { get; set; } = null!;
    public Guid CreatedId { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public Guid? ModifiedId { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
}
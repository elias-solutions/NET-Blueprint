using NET.Backend.Blueprint.Api.Entities.Base;

namespace NET.Backend.Blueprint.Api.Entities;

public class Address : EntityBase
{
    public string Street { get; set; } = default!;
    public string Number { get; set; } = default!;
    public string City { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public Guid PersonId { get; set; } = default!;
    public Person Person { get; set; } = default!;
    public Guid CreatedId { get; set; } = default!;
    public DateTimeOffset CreatedDate { get; set; }
    public Guid? ModifiedId { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
}
using NET.Backend.Blueprint.Entities.Base;

namespace NET.Backend.Blueprint.Entities;

public class Address : EntityBase
{
    public string Street { get; set; } = default!;
    public string Number { get; set; } = default!;
    public string City { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public Guid PersonId { get; set; } = default!;
    public Person Person { get; set; } = default!;
}
using NET.Backend.Blueprint.Domain.Entities.Base;

namespace NET.Backend.Blueprint.Domain.Entities;

public class Person : EntityBase
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required DateOnly Birthday { get; set; }

    public Guid CreatedId { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public Guid? ModifiedId { get; set; }

    public DateTimeOffset? ModifiedDate { get; set; }

    public ICollection<Address> Addresses { get; set; } = [];
}

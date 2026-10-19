using NET.Backend.Blueprint.Domain.Entities.Base;
using NET.Backend.Blueprint.Domain.Enums;

namespace NET.Backend.Blueprint.Domain.Entities;

public class Person : EntityBase
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public DateOnly? Birthday { get; set; }

    public Gender? Gender { get; set; }

    public string? Description { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public PersonType? Type { get; set; }

    public Guid CreatedId { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public Guid? ModifiedId { get; set; }

    public DateTimeOffset? ModifiedDate { get; set; }

    public ICollection<Address> Addresses { get; set; } = [];
}

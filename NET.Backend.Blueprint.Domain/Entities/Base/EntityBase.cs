namespace NET.Backend.Blueprint.Domain.Entities.Base;

public abstract class EntityBase
{
    public Guid Id { get; set; }

    public Guid Version { get; set; }
}

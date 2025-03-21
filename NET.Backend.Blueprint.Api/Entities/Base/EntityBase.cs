namespace NET.Backend.Blueprint.Api.Entities.Base
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }
        
        public Guid Version { get; set; }
    }
}

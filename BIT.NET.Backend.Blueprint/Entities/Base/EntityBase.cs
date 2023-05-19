namespace BIT.NET.Backend.Blueprint.Entities.Base
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTimeOffset Modified{ get; set; }

        public Guid ModifiedBy { get; set; }
    }
}

using NET.Backend.Blueprint.Api.Authorization;
using NET.Backend.Blueprint.Api.Entities.Base;

namespace NET.Backend.Blueprint.Api.Entities
{
    public class Person : EntityBase
    {
        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public DateOnly Birthday { get; set; }

        public Guid CreatedId { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public Guid? ModifiedId { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public ICollection<Address> Addresses { get; set; } = default!;
    }
}

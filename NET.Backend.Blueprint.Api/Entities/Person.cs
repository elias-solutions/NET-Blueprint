using NET.Backend.Blueprint.Api.Entities.Base;

namespace NET.Backend.Blueprint.Api.Entities
{
    public class Person : EntityBase
    {
        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public DateTimeOffset Birthday { get; set; }

        public ICollection<Address> Addresses { get; set; } = default!;
    }
}

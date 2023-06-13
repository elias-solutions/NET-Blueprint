using BIT.NET.Backend.Blueprint.Entities.Base;

namespace BIT.NET.Backend.Blueprint.Entities
{
    public class Person : EntityBase
    {
        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public DateTimeOffset Birthday { get; set; }

        public ICollection<Address> Addresses { get; set; } = default!;
    }
}

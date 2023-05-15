using BIT.NET.Backend.Blueprint.Authorization;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments
{
    public static class TestUsers
    {
        public static User Admin { get; } = new (Guid.Parse("E3D3D338-2053-4CE9-877D-DE4563FE321C"), Roles.Admin, $"{Roles.Admin}@{Roles.Admin}.ch", new[] { Roles.Admin });

        public static User Standard { get; } = new(Guid.Parse("E3D3D338-2053-4CE9-877D-DE4563FE321D"), Roles.Standard, $"{Roles.Standard}@{Roles.Standard}.ch", new[] { Roles.Standard });
    }
}

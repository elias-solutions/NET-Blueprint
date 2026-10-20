using NET.Backend.Blueprint.Web.Authorization;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;

public static class TestUsers
{
    public static TestUser Admin { get; } = new(Guid.Parse("E3D3D338-2053-4CE9-877D-DE4563FE321C"), "admin", [Roles.Admin]);

    public static TestUser Writer { get; } = new(Guid.Parse("E3D3D338-2053-4CE9-877D-DE4563FE321E"), "writer", [Roles.Writer]);

    public static TestUser Reader { get; } = new(Guid.Parse("E3D3D338-2053-4CE9-877D-DE4563FE321D"), "reader", [Roles.Reader]);
}

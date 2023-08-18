namespace NET.Backend.Blueprint.Authorization;

public record User(Guid Id, string UserName, string Email, string[] Roles);
namespace NET.Backend.Blueprint.Api.Authorization;

public record User(Guid Id, string UserName, string Email, string[] Roles);
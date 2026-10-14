namespace NET.Backend.Blueprint.Web.Authorization;

public record User(Guid Id, string UserName, string Email, string[] Roles);

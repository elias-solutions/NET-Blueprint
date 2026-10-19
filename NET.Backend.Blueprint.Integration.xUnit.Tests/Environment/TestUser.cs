namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;

/// <summary>A Keycloak user as the access token describes it: <see cref="Id"/> becomes the <c>sub</c> claim.</summary>
public record TestUser(Guid Id, string UserName, string[] Roles);

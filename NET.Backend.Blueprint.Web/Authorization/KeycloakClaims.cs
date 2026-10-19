namespace NET.Backend.Blueprint.Web.Authorization;

/// <summary>Claims of the access tokens issued by the Keycloak realm "blueprint".</summary>
public static class KeycloakClaims
{
    public const string UserName = "preferred_username";
    public const string Roles = "roles";
}

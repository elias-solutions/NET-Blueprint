namespace NET.Backend.Blueprint.Web.Authorization;

public static class Policies
{
    /// <summary>Every role may read, only <see cref="Roles.Admin"/> may write (see <see cref="ApiAccessHandler"/>).</summary>
    public const string ApiAccess = "ApiAccess";
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace NET.Backend.Blueprint.Web.Authorization;

/// <summary>
/// Grants reading requests to every blueprint role and all other requests to admins only,
/// so a new write endpoint is protected without further configuration.
/// </summary>
public sealed class ApiAccessHandler : AuthorizationHandler<ApiAccessRequirement, HttpContext>
{
    private static readonly string[] ReadRoles = [Roles.Standard, Roles.Admin];
    private static readonly string[] WriteRoles = [Roles.Admin];

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, ApiAccessRequirement requirement, HttpContext httpContext)
    {
        if (RequiredRoles(httpContext.Request.Method).Any(context.User.IsInRole))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }

    private static string[] RequiredRoles(string method) =>
        HttpMethods.IsGet(method) || HttpMethods.IsHead(method) ? ReadRoles : WriteRoles;
}

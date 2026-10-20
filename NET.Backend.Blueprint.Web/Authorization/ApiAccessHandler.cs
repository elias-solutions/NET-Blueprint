using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace NET.Backend.Blueprint.Web.Authorization;

public sealed class ApiAccessHandler : AuthorizationHandler<ApiAccessRequirement, HttpContext>
{
    private static readonly string[] ReadRoles = [Roles.Reader, Roles.Writer, Roles.Admin];
    private static readonly string[] WriteRoles = [Roles.Writer, Roles.Admin];
    private static readonly string[] DeleteRoles = [Roles.Admin];

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, ApiAccessRequirement requirement, HttpContext httpContext)
    {
        if (RequiredRoles(httpContext.Request.Method).Any(context.User.IsInRole))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }

    private static string[] RequiredRoles(string method)
    {
        if (HttpMethods.IsGet(method) || HttpMethods.IsHead(method))
        {
            return ReadRoles;
        }

        return HttpMethods.IsDelete(method) ? DeleteRoles : WriteRoles;
    }
}

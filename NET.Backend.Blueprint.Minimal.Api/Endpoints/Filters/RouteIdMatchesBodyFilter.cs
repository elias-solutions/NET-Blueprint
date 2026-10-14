namespace NET.Backend.Blueprint.Minimal.Api.Endpoints.Filters;

/// <summary>
/// Rejects requests whose <c>{id}</c> route value differs from the id carried in the request body.
/// </summary>
public sealed class RouteIdMatchesBodyFilter<TRequest>(Func<TRequest, Guid> bodyIdSelector) : IEndpointFilter
{
    private const string RouteIdName = "id";

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var routeValue = context.HttpContext.GetRouteValue(RouteIdName)?.ToString();
        var request = context.Arguments.OfType<TRequest>().Single();
        var bodyId = bodyIdSelector(request);

        if (!Guid.TryParse(routeValue, out var routeId) || routeId != bodyId)
        {
            return TypedResults.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Id mismatch",
                detail: $"Provided route id '{routeValue}' and body id '{bodyId}' are not equal.");
        }

        return await next(context);
    }
}

public static class RouteIdMatchesBodyFilterExtensions
{
    public static RouteHandlerBuilder RequireRouteIdMatchesBody<TRequest>(this RouteHandlerBuilder builder, Func<TRequest, Guid> bodyIdSelector)
    {
        return builder
            .AddEndpointFilter(new RouteIdMatchesBodyFilter<TRequest>(bodyIdSelector))
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}

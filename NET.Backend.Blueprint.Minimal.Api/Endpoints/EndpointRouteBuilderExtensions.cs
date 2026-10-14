using NET.Backend.Blueprint.Minimal.Api.Endpoints.V1;
using NET.Backend.Blueprint.Minimal.Api.Endpoints.V2;
using NET.Backend.Blueprint.Minimal.Api.RateLimiting;
using NET.Backend.Blueprint.Web.Authorization;

namespace NET.Backend.Blueprint.Minimal.Api.Endpoints;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapBlueprintEndpoints(this IEndpointRouteBuilder app)
    {
        var persons = app.NewVersionedApi("Persons");
        persons.MapPersonsEndpointsV1();
        persons.MapPersonsEndpointsV2();

        var addresses = app.NewVersionedApi("Addresses");
        addresses.MapAddressesEndpointsV1();

        return app;
    }

    public static RouteGroupBuilder WithBlueprintConventions(this RouteGroupBuilder group)
    {
        group
            .RequireAuthorization(Policies.Admin)
            .RequireRateLimiting(FixedWindowRateLimitSettings.PolicyName)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return group;
    }
}

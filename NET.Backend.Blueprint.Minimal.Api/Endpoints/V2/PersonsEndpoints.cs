using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Http.HttpResults;

namespace NET.Backend.Blueprint.Minimal.Api.Endpoints.V2;

public static class PersonsEndpoints
{
    public static void MapPersonsEndpointsV2(this IVersionedEndpointRouteBuilder api)
    {
        var group = api.MapGroup("api/v{version:apiVersion}/persons")
            .HasApiVersion(2.0)
            .WithBlueprintConventions();

        group.MapGet("", GetPersons)
            .WithSummary("Returns the persons in the v2 representation.");
    }

    private static Ok<string> GetPersons() => TypedResults.Ok("Jonas Elias");
}

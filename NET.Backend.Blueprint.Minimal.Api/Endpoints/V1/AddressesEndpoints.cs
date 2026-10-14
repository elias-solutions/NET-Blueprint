using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NET.Backend.Blueprint.Application.CQRS;
using NET.Backend.Blueprint.Application.CQRS.Commands;
using NET.Backend.Blueprint.Application.CQRS.Queries;
using NET.Backend.Blueprint.Application.Model.Commands;
using NET.Backend.Blueprint.Application.Model.Queries;
using NET.Backend.Blueprint.Minimal.Api.Endpoints.Filters;

namespace NET.Backend.Blueprint.Minimal.Api.Endpoints.V1;

public static class AddressesEndpoints
{
    public static void MapAddressesEndpointsV1(this IVersionedEndpointRouteBuilder api)
    {
        var group = api.MapGroup("api/v{version:apiVersion}/addresses")
            .HasApiVersion(1.0)
            .WithBlueprintConventions();

        group.MapPut("{id}", UpdateAddressAsync)
            .RequireRouteIdMatchesBody<UpdateAddressRequest>(request => request.AddressId)
            .WithSummary("Updates an address and returns its new state.");
    }

    private static async Task<Ok<GetAddressResponse>> UpdateAddressAsync(
        Guid id,
        UpdateAddressRequest request,
        [FromServices] ICommandHandler<UpdateAddressCommand> updateHandler,
        [FromServices] IQueryHandler<GetAddressResponseByIdQuery, GetAddressResponse> queryHandler,
        CancellationToken cancellationToken)
    {
        await updateHandler.HandleAsync(new UpdateAddressCommand(request), cancellationToken);
        return TypedResults.Ok(await queryHandler.HandleAsync(new GetAddressResponseByIdQuery(id), cancellationToken));
    }
}

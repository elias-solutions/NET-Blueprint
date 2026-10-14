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

public static class PersonsEndpoints
{
    public static void MapPersonsEndpointsV1(this IVersionedEndpointRouteBuilder api)
    {
        var group = api.MapGroup("api/v{version:apiVersion}/persons")
            .HasApiVersion(1.0)
            .WithBlueprintConventions();

        group.MapGet("", GetPersonsAsync)
            .WithSummary("Returns all persons including their addresses.");

        group.MapGet("{id}", GetPersonByIdAsync)
            .WithSummary("Returns a single person including its addresses.");

        group.MapPost("", CreatePersonAsync)
            .WithSummary("Creates a person with its addresses.");

        group.MapPut("{id}", UpdatePersonAsync)
            .RequireRouteIdMatchesBody<UpdatePersonRequest>(request => request.Id)
            .WithSummary("Updates a person and synchronizes its addresses.");

        group.MapDelete("{id}", DeletePersonByIdAsync)
            .WithSummary("Deletes a person including its addresses.");
    }

    private static async Task<Ok<IReadOnlyList<GetPersonResponse>>> GetPersonsAsync(
        [FromServices] IQueryHandler<GetPersonsResponseQuery, IReadOnlyList<GetPersonResponse>> handler,
        CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await handler.HandleAsync(new GetPersonsResponseQuery(), cancellationToken));
    }

    private static async Task<Ok<GetPersonResponse>> GetPersonByIdAsync(
        Guid id,
        [FromServices] IQueryHandler<GetPersonResponseByIdQuery, GetPersonResponse> handler,
        CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await handler.HandleAsync(new GetPersonResponseByIdQuery(id), cancellationToken));
    }

    private static async Task<Ok<CreatePersonResponse>> CreatePersonAsync(
        CreatePersonRequest request,
        [FromServices] ICommandHandler<CreatePersonCommand, CreatePersonResponse> handler,
        CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await handler.HandleAsync(new CreatePersonCommand(request), cancellationToken));
    }

    private static async Task<Ok> UpdatePersonAsync(
        Guid id,
        UpdatePersonRequest request,
        [FromServices] ICommandHandler<UpdatePersonCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new UpdatePersonCommand(request), cancellationToken);
        return TypedResults.Ok();
    }

    private static async Task<Ok> DeletePersonByIdAsync(
        Guid id,
        [FromServices] ICommandHandler<DeletePersonByIdCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new DeletePersonByIdCommand(id), cancellationToken);
        return TypedResults.Ok();
    }
}

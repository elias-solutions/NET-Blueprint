namespace NET.Backend.Blueprint.Application.Exceptions;

public sealed class EntityNotFoundException(string entityName, Guid id)
    : BlueprintException($"No {entityName} found", $"No {entityName} with id '{id}' found.");

namespace NET.Backend.Blueprint.Application.Exceptions;

public sealed class VersionConflictException()
    : BlueprintException("Entity version conflict", "Entity has been updated through other user.");

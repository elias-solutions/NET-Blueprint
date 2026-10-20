namespace NET.Backend.Blueprint.Application.Exceptions;

public abstract class BlueprintException(string title, string detail) : Exception(detail)
{
    public string Title { get; } = title;
}

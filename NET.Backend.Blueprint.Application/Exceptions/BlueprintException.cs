namespace NET.Backend.Blueprint.Application.Exceptions;

/// <summary>
/// Base for expected business failures that the web layer translates into problem details.
/// </summary>
public abstract class BlueprintException(string title, string detail) : Exception(detail)
{
    public string Title { get; } = title;
}

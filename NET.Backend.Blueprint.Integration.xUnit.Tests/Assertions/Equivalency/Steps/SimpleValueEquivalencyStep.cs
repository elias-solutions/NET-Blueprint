namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions.Equivalency.Steps;

internal sealed class SimpleValueEquivalencyStep : IEquivalencyStep
{
    private static readonly HashSet<Type> SimpleTypes =
    [
        typeof(string), typeof(decimal), typeof(Guid), typeof(Uri),
        typeof(DateTime), typeof(DateTimeOffset), typeof(DateOnly), typeof(TimeOnly), typeof(TimeSpan)
    ];

    public bool CanHandle(object expected)
    {
        var type = expected.GetType();
        return type.IsPrimitive || type.IsEnum || SimpleTypes.Contains(type);
    }

    public void Compare(object actual, object expected, string path, EquivalencyValidator validator, ICollection<Difference> differences)
    {
        if (!expected.Equals(actual))
        {
            differences.Add(EquivalencyValidator.Mismatch(path, actual, expected));
        }
    }
}

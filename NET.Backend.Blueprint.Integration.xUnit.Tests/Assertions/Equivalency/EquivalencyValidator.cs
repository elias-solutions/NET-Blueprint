using NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions.Equivalency.Steps;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions.Equivalency;

internal sealed record Difference(string Path, string Reason);

internal sealed class EquivalencyValidator(IReadOnlySet<string> excludedPaths)
{
    private static readonly IEquivalencyStep[] Steps =
    [
        new SimpleValueEquivalencyStep(),
        new DictionaryEquivalencyStep(),
        new CollectionEquivalencyStep(),
        new MemberwiseEquivalencyStep()
    ];

    public void Compare(object? actual, object? expected, string path, ICollection<Difference> differences)
    {
        if (excludedPaths.Contains(MemberPath.Normalize(path))) return;

        if (actual is null || expected is null)
        {
            if (actual is not null || expected is not null)
            {
                differences.Add(Mismatch(path, actual, expected));
            }

            return;
        }

        Steps.First(step => step.CanHandle(expected)).Compare(actual, expected, path, this, differences);
    }

    public bool AreEquivalent(object? actual, object? expected, string path)
    {
        var differences = new List<Difference>();
        Compare(actual, expected, path, differences);
        return differences.Count == 0;
    }

    public static Difference Mismatch(string path, object? actual, object? expected) =>
        new(path, $"expected {ValueFormatter.Format(expected)}, but found {ValueFormatter.Format(actual)}");
}

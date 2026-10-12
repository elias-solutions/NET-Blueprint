namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions.Equivalency;

internal static class EquivalencyAssertion
{
    public static void Assert(object? actual, object? expected, IReadOnlySet<string> excludedPaths, string expression)
    {
        var differences = new List<Difference>();
        new EquivalencyValidator(excludedPaths).Compare(actual, expected, string.Empty, differences);

        if (differences.Count == 0) return;

        var details = differences.Select(difference =>
            $"  - {MemberPath.Combine(expression, difference.Path)}: {difference.Reason}");

        Xunit.Assert.Fail(
            $"Expected {expression} to be equivalent to the expectation, but found {differences.Count} difference(s):" +
            System.Environment.NewLine +
            string.Join(System.Environment.NewLine, details));
    }
}

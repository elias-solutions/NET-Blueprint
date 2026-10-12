using NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions.Equivalency;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions;

public class CollectionAssertions<TItem>(IEnumerable<TItem>? subject, string expression)
{
    public void BeEquivalentTo(
        IEnumerable<TItem>? expected,
        Func<EquivalencyOptions<TItem>, EquivalencyOptions<TItem>>? configure = null)
    {
        var itemOptions = (configure ?? (options => options))(new EquivalencyOptions<TItem>());
        var excludedPaths = itemOptions.ExcludedPaths
            .Select(path => MemberPath.Combine(MemberPath.AnyItem(string.Empty), path))
            .ToHashSet();

        EquivalencyAssertion.Assert(subject, expected, excludedPaths, expression);
    }
}

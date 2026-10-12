using System.Linq.Expressions;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions.Equivalency;

public class EquivalencyOptions<T>
{
    private readonly HashSet<string> _excludedPaths = [];

    internal IReadOnlySet<string> ExcludedPaths => _excludedPaths;

    public EquivalencyOptions<T> Excluding(Expression<Func<T, object?>> member) =>
        Exclude(MemberPath.Of(member));

    public NestedEquivalencyOptions<T, TItem> For<TItem>(Expression<Func<T, IEnumerable<TItem>>> collection) =>
        new(this, MemberPath.AnyItem(MemberPath.Of(collection)));

    internal EquivalencyOptions<T> Exclude(string path)
    {
        _excludedPaths.Add(path);
        return this;
    }
}

public class NestedEquivalencyOptions<T, TItem>(EquivalencyOptions<T> parent, string itemPath)
{
    public EquivalencyOptions<T> Exclude(Expression<Func<TItem, object?>> member) =>
        parent.Exclude(MemberPath.Combine(itemPath, MemberPath.Of(member)));
}

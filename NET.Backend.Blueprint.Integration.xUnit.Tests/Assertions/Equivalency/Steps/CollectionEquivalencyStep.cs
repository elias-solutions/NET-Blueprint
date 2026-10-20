using System.Collections;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions.Equivalency.Steps;

internal sealed class CollectionEquivalencyStep : IEquivalencyStep
{
    public bool CanHandle(object expected) => expected is IEnumerable;

    public void Compare(object actual, object expected, string path, EquivalencyValidator validator, ICollection<Difference> differences)
    {
        if (actual is not IEnumerable actualEnumerable)
        {
            differences.Add(new Difference(path, $"expected a collection, but found {actual.GetType().Name}"));
            return;
        }

        var actualItems = actualEnumerable.Cast<object?>().ToList();
        var expectedItems = ((IEnumerable)expected).Cast<object?>().ToList();

        if (actualItems.Count != expectedItems.Count)
        {
            differences.Add(new Difference(path, $"expected {expectedItems.Count} item(s), but found {actualItems.Count}"));
            return;
        }

        var unmatched = Enumerable.Range(0, actualItems.Count).ToList();

        for (var index = 0; index < expectedItems.Count; index++)
        {
            var itemPath = MemberPath.Item(path, index);
            var match = unmatched.FindIndex(candidate => validator.AreEquivalent(actualItems[candidate], expectedItems[index], itemPath));

            if (match >= 0)
            {
                unmatched.RemoveAt(match);
            }
            else
            {
                ReportClosestDifferences(actualItems[index], expectedItems[index], itemPath, validator, differences);
            }
        }
    }

    private static void ReportClosestDifferences(object? actual, object? expected, string itemPath, EquivalencyValidator validator, ICollection<Difference> differences)
    {
        var itemDifferences = new List<Difference>();
        validator.Compare(actual, expected, itemPath, itemDifferences);

        if (itemDifferences.Count == 0)
        {
            differences.Add(new Difference(itemPath, "no equivalent item found in the actual collection"));
            return;
        }

        foreach (var difference in itemDifferences)
        {
            differences.Add(difference);
        }
    }
}

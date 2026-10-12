using System.Collections;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions.Equivalency.Steps;

internal sealed class DictionaryEquivalencyStep : IEquivalencyStep
{
    public bool CanHandle(object expected) => expected is IDictionary;

    public void Compare(object actual, object expected, string path, EquivalencyValidator validator, ICollection<Difference> differences)
    {
        if (actual is not IDictionary actualDictionary)
        {
            differences.Add(new Difference(path, $"expected a dictionary, but found {actual.GetType().Name}"));
            return;
        }

        var expectedDictionary = (IDictionary)expected;

        foreach (DictionaryEntry entry in expectedDictionary)
        {
            var entryPath = MemberPath.Item(path, entry.Key);

            if (actualDictionary.Contains(entry.Key))
            {
                validator.Compare(actualDictionary[entry.Key], entry.Value, entryPath, differences);
            }
            else
            {
                differences.Add(new Difference(entryPath, "expected key is missing"));
            }
        }

        foreach (var key in actualDictionary.Keys.Cast<object>().Where(key => !expectedDictionary.Contains(key)))
        {
            differences.Add(new Difference(MemberPath.Item(path, key), "unexpected key"));
        }
    }
}

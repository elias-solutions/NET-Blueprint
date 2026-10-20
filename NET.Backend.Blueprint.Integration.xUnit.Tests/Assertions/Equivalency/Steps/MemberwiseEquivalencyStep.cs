using System.Reflection;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions.Equivalency.Steps;

internal sealed class MemberwiseEquivalencyStep : IEquivalencyStep
{
    private const BindingFlags PublicInstance = BindingFlags.Public | BindingFlags.Instance;

    public bool CanHandle(object expected) => true;

    public void Compare(object actual, object expected, string path, EquivalencyValidator validator, ICollection<Difference> differences)
    {
        var properties = expected.GetType()
            .GetProperties(PublicInstance)
            .Where(property => property.CanRead && property.GetIndexParameters().Length == 0);

        foreach (var expectedProperty in properties)
        {
            var memberPath = MemberPath.Member(path, expectedProperty.Name);
            var actualProperty = actual.GetType().GetProperty(expectedProperty.Name, PublicInstance);

            if (actualProperty is null)
            {
                differences.Add(new Difference(memberPath, $"member is missing on {actual.GetType().Name}"));
                continue;
            }

            validator.Compare(actualProperty.GetValue(actual), expectedProperty.GetValue(expected), memberPath, differences);
        }
    }
}

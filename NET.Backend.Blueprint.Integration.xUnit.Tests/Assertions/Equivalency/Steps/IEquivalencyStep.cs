namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions.Equivalency.Steps;

internal interface IEquivalencyStep
{
    bool CanHandle(object expected);

    void Compare(object actual, object expected, string path, EquivalencyValidator validator, ICollection<Difference> differences);
}

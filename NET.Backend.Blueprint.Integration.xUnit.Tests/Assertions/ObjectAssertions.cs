using NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions.Equivalency;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions;

public class ObjectAssertions<T>(T? subject, string expression)
{
    public void Be(T expected)
    {
        if (!EqualityComparer<T?>.Default.Equals(subject, expected))
        {
            Assert.Fail($"Expected {expression} to be {ValueFormatter.Format(expected)}, but found {ValueFormatter.Format(subject)}.");
        }
    }

    public void NotBe(T unexpected)
    {
        if (EqualityComparer<T?>.Default.Equals(subject, unexpected))
        {
            Assert.Fail($"Expected {expression} not to be {ValueFormatter.Format(unexpected)}, but it was.");
        }
    }

    public void BeEquivalentTo<TExpectation>(
        TExpectation expected,
        Func<EquivalencyOptions<TExpectation>, EquivalencyOptions<TExpectation>>? configure = null)
    {
        var options = (configure ?? (options => options))(new EquivalencyOptions<TExpectation>());
        EquivalencyAssertion.Assert(subject, expected, options.ExcludedPaths, expression);
    }
}

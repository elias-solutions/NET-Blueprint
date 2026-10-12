using System.Runtime.CompilerServices;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions;

public static class ShouldExtensions
{
    public static ObjectAssertions<T> Should<T>(
        this T? subject,
        [CallerArgumentExpression(nameof(subject))] string expression = "") =>
        new(subject, expression);

    public static CollectionAssertions<TItem> Should<TItem>(
        this IEnumerable<TItem>? subject,
        [CallerArgumentExpression(nameof(subject))] string expression = "") =>
        new(subject, expression);
}

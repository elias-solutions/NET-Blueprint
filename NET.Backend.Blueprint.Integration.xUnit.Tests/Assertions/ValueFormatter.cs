using System.Globalization;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions;

internal static class ValueFormatter
{
    public static string Format(object? value) => value switch
    {
        null => "<null>",
        string text => $"\"{text}\"",
        IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
        _ => value.ToString() ?? value.GetType().Name
    };
}

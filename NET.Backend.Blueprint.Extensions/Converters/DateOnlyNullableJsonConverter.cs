using System.Text.Json;
using System.Text.Json.Serialization;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Converters;

public class DateOnlyNullableJsonConverter : JsonConverter<DateOnly?>
{
    private const string Format = "yyyy-MM-dd";

    public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        var value = reader.GetString();
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (DateOnly.TryParseExact(value, Format, out var date))
        {
            return date;
        }

        throw new JsonException($"Invalid date format. Expected format: {Format}");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value.ToString(Format));
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.SQlite.Extensions;

public static class StringExtensions
{
    private const string ApplicationJson = MediaTypeNames.Application.Json;

    public static T ReadFromJson<T>(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            throw new ArgumentException($"Argument '{nameof(source)}' is null or empty");
        }

        var obj = JsonSerializer.Deserialize<T>(source, new JsonSerializerOptions());
        if (obj == null)
        {
            throw new ArgumentException($"Could not deserialize '{nameof(source)}'");
        }

        return obj;
    }

    public static StringContent ToStringContent(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException($"Argument '{nameof(value)}' is null or empty");
        }

        return new StringContent(value, Encoding.UTF8, ApplicationJson);
    }

    public static async Task<string> ToJsonStringContentAsync(this string jsonFileName)
    {
        if (string.IsNullOrEmpty(jsonFileName))
        {
            throw new ArgumentException($"{nameof(jsonFileName)} is null or empty.");
        }

        var sourceAssembly = Assembly.GetExecutingAssembly();
        var resourceNames = sourceAssembly.GetManifestResourceNames();
        var jsonFullFilePath = resourceNames.Single(resource => resource.EndsWith(jsonFileName));

        await using var stream = sourceAssembly.GetManifestResourceStream(jsonFullFilePath);
        if (stream == null)
        {
            throw new ArgumentException($"No resource file found '{jsonFileName}'.");
        }

        using var reader = new StreamReader(stream);
        var jsonStringContent = await reader.ReadToEndAsync();
        return jsonStringContent;
    }
}
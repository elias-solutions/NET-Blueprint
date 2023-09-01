using System.Reflection;
using NET.Backend.Blueprint.Extensions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Extensions;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;

public class EmbeddedJsonResourceProvider
{
    private readonly string _namespaceOfTest;

    public EmbeddedJsonResourceProvider(string namespaceOfTest)
    {
        if (string.IsNullOrEmpty(namespaceOfTest)) throw new ArgumentNullException(nameof(namespaceOfTest), "Argument should not be null or empty.");

        _namespaceOfTest = namespaceOfTest;
    }

    public async Task<T> CreateObjectByResourceAsync<T>(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(nameof(fileName), "Argument should not be null or empty.");

        var jsonString = await ReadJsonString(fileName);
        var result = jsonString.ReadFromJson<T>();

        if (result == null)
        {
            throw new ArgumentException("Could not deserialize object.");
        }

        return result;
    }

    public async Task<StringContent> CreateHttpContentByResourceAsync(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(nameof(fileName), "Argument should not be null or empty.");

        var jsonString = await ReadJsonString(fileName);
        return jsonString.ToStringContent();
    }

    private async Task<string> ReadJsonString(string fileName)
    {
        var fullResourceFileName = $"{_namespaceOfTest}.Resources.{fileName}";
        var jsonString = await ReadEmbeddedResourceAsync(fullResourceFileName);
        return jsonString;
    }

    private async Task<string> ReadEmbeddedResourceAsync(string jsonResourceFileName)
    {
        var sourceAssembly = Assembly.GetExecutingAssembly();
        var jsonFullFilePath = sourceAssembly.GetManifestResourceNames().Single(resource => resource.Equals(jsonResourceFileName));
        await using var stream = sourceAssembly.GetManifestResourceStream(jsonFullFilePath);
        if (stream == null)
        {
            throw new ArgumentException($"No resource file found '{jsonResourceFileName}'.");
        }

        using var reader = new StreamReader(stream);
        var jsonString = await reader.ReadToEndAsync();
        return jsonString;
    }
}
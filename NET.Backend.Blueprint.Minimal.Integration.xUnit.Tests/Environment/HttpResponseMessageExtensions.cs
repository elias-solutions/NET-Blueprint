using System.Net.Http.Json;

namespace NET.Backend.Blueprint.Minimal.Integration.xUnit.Tests.Environment;

public static class HttpResponseMessageExtensions
{
    public static async Task<T> ReadAsync<T>(this HttpResponseMessage source)
    {
        return await source.Content.ReadFromJsonAsync<T>()
               ?? throw new InvalidOperationException($"Could not deserialize response into '{typeof(T).Name}'.");
    }
}

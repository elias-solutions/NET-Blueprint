using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Converters;

namespace NET.Backend.Blueprint.Extensions
{
    public static class HttpContentExtensions
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true,
            Converters =
            {
                new JsonStringEnumConverter(),
                new DateOnlyJsonConverter(),
                new DateOnlyNullableJsonConverter(),
            }
        };

        public static async Task<T> ReadAsync<T>(this HttpContent source)
        {
            var result = await source.ReadFromJsonAsync<T>(Options);

            if (result == null)
            {
                throw new Exception($"Could not convert json into '{nameof(T)}'");
            }

            return result;
        }
    }
}
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using System;

namespace BIT.NET.Backend.Blueprint.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static DateTimeOffset ToDatabaseDateTimeOffset(this DateTimeOffset source)
        {
            var dateTimeOffset = new DateTimeOffset(source.Year, source.Month, source.Day, source.Minute, source.Second, 0, TimeSpan.Zero);
            return dateTimeOffset;
        }
    }

    public static class HttpContentExtensions
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true,
            IncludeFields = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
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
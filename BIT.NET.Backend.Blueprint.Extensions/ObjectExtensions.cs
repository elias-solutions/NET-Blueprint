using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace BIT.NET.Backend.Blueprint.Extensions;

public static class ObjectExtensions
{
    private const string ApplicationJson = MediaTypeNames.Application.Json;
    
    public static StringContent ToStringContent(this object value)
    {
        if (value == null)
        {
            throw new ArgumentException($"Argument '{nameof(value)}' null");
        }
            
        return new StringContent(value.ToJson(), Encoding.UTF8, ApplicationJson);
    }

    public static string ToJson(this object value)
    {
        if (value == null)
        {
            throw new ArgumentException($"Argument '{nameof(value)}' null");
        }

        return JsonSerializer.Serialize(value);
    }
}
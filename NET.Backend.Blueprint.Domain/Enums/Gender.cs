using System.Text.Json.Serialization;

namespace NET.Backend.Blueprint.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<Gender>))]
public enum Gender
{
    Male,
    Female
}

using System.Text.Json.Serialization;

namespace NET.Backend.Blueprint.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<PersonType>))]
public enum PersonType
{
    Freelancer,
    Employee
}

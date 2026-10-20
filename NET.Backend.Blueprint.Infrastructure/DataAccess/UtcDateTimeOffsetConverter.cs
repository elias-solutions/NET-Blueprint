using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NET.Backend.Blueprint.Infrastructure.DataAccess;

public class UtcDateTimeOffsetConverter() : ValueConverter<DateTimeOffset, DateTimeOffset>(
    value => value.ToUniversalTime(),
    value => value);

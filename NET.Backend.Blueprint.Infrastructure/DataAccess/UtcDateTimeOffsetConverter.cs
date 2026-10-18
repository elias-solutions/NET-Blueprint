using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NET.Backend.Blueprint.Infrastructure.DataAccess;

/// <summary>PostgreSQL <c>timestamptz</c> only accepts UTC offsets; client-supplied offsets are normalized on write.</summary>
public class UtcDateTimeOffsetConverter() : ValueConverter<DateTimeOffset, DateTimeOffset>(
    value => value.ToUniversalTime(),
    value => value);

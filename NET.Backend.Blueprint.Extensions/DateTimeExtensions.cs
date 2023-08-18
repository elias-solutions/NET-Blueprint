namespace NET.Backend.Blueprint.Extensions;

public static class DateTimeExtensions
{
    public static DateTimeOffset ToUtcDateTimeOffset(this DateTime source)
    {
        var dateTimeWithKind = new DateTime(source.Year, source.Month, source.Day, source.Hour, source.Minute, source.Second, DateTimeKind.Utc);
        return new DateTimeOffset(dateTimeWithKind);
    }
}
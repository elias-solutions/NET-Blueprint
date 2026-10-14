namespace NET.Backend.Blueprint.Minimal.Api.RateLimiting;

public class FixedWindowRateLimitSettings
{
    public const string SectionName = "RateLimiting";
    public const string PolicyName = "fixed";

    public int PermitLimit { get; init; } = 4;

    public TimeSpan Window { get; init; } = TimeSpan.FromSeconds(5);

    public int QueueLimit { get; init; } = 2;
}

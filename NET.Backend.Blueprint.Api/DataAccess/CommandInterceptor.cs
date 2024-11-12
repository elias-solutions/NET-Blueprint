using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;
using StackTrace = System.Diagnostics.StackTrace;

namespace NET.Backend.Blueprint.Api.DataAccess;

public class CommandInterceptor(ILogger<CommandInterceptor> logger) : DbCommandInterceptor
{
    private const bool IsActive = true;
    private const int MaxDuration = 1000;

    public CommandInterceptor(IServiceProvider serviceProvider) : this(serviceProvider.GetService<ILogger<CommandInterceptor>>()!)
    {
    }

    public override async ValueTask<DbDataReader> ReaderExecutedAsync(
        DbCommand command, 
        CommandExecutedEventData eventData, 
        DbDataReader result,
        CancellationToken cancellationToken = new())
    {
        if (!IsActive)
        {
            return await base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
        }

        var startTime = Stopwatch.GetTimestamp();
        var executionResult = await base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
        var durationMilliseconds = Stopwatch.GetElapsedTime(startTime).TotalMilliseconds;

        if (durationMilliseconds > MaxDuration)
        {
            LogSlowQuery(command.CommandText, MaxDuration, durationMilliseconds);
        }

        return executionResult;
    }

    private void LogSlowQuery(string commandText, int maxMilliSeconds, double duration)
    {
        var fullStackTrace = new StackTrace(true);

        if (fullStackTrace != null)
        {
            var results = FilterStackTrace(fullStackTrace).Take(15).ToList();
            if (results.Any())
            {
                var lines = string.Join(", ", results);
                logger.LogWarning($"Slow query: {commandText} {duration}ms (Max. {MaxDuration}ms) Stacktrace: {lines}");
            }
        }
    }

    private static IEnumerable<string> FilterStackTrace(StackTrace fullStackTrace)
    {
        foreach (var frame in fullStackTrace.GetFrames())
        {
            var method = frame.GetMethod();
            if (method != null)
            {
                var declaringType = method.DeclaringType?.FullName;
                if (declaringType != null && declaringType.StartsWith("NET.Backend.Blueprint"))
                {
                    var fileName = frame.GetFileName() ?? "Unknown file";
                    var lineNumber = frame.GetFileLineNumber() != -1 ? frame.GetFileLineNumber() : 0;
                    yield return $"{declaringType}.{method.Name} (at {fileName}.{lineNumber}";
                }
            }
        }
    }
}
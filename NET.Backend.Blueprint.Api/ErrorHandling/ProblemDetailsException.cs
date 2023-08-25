using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace NET.Backend.Blueprint.Api.ErrorHandling;

public class ProblemDetailsException : Exception
{
    public ProblemDetailsException(HttpStatusCode statusCode, string title, string details, params (string key, object value)[] extensions)
    {
        ProblemDetails = new ProblemDetails
        {
            Title = $"{statusCode} - {title}",
            Detail = details,
            Status = (int)statusCode
        };

        var keyValuePairs = extensions.Select(tuple => new KeyValuePair<string, object>(tuple.key, tuple.value));
        foreach (var keyValuePair in keyValuePairs)
        {
            ProblemDetails.Extensions.Add(keyValuePair!);
        }
    }

    public ProblemDetails ProblemDetails { get; }
}
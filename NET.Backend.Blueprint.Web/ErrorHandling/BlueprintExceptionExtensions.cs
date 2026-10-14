using System.Net;
using Microsoft.AspNetCore.Mvc;
using NET.Backend.Blueprint.Application.Exceptions;

namespace NET.Backend.Blueprint.Web.ErrorHandling;

public static class BlueprintExceptionExtensions
{
    private const HttpStatusCode StatusCode = HttpStatusCode.BadRequest;

    public static ProblemDetails ToProblemDetails(this BlueprintException exception)
    {
        return new ProblemDetails
        {
            Title = $"{StatusCode} - {exception.Title}",
            Detail = exception.Message,
            Status = (int)StatusCode
        };
    }
}

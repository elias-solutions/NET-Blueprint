using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using NET.Backend.Blueprint.Application.Exceptions;

namespace NET.Backend.Blueprint.Web.ErrorHandling;

public class BlueprintExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not BlueprintException blueprintException)
        {
            return false;
        }

        var problemDetails = blueprintException.ToProblemDetails();
        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception
        });
    }
}

using System.Net.Mime;
using System.Text.Json;
using NET.Backend.Blueprint.Application.Exceptions;
using NET.Backend.Blueprint.Web.ErrorHandling;

namespace NET.Backend.Blueprint.Api.ErrorHandling
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (BlueprintException exception)
            {
                var problemDetails = exception.ToProblemDetails();
                context.Response.Clear();
                context.Response.ContentType = MediaTypeNames.Application.Json;

                if (problemDetails.Status != null)
                {
                    context.Response.StatusCode = problemDetails.Status.Value;
                }

                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
            }
        }
    }
}

using System.Net;

namespace TrueCodeExample.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "Unhandled exception");

        var (statusCode, message) = exception switch
        {
            ArgumentException argumentException =>
                (HttpStatusCode.BadRequest, argumentException.Message),
            KeyNotFoundException keyNotFoundException =>
                (HttpStatusCode.NotFound, keyNotFoundException.Message),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsJsonAsync(new { error = message });
    }
}

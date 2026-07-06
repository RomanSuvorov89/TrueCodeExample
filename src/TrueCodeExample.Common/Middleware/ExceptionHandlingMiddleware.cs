using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TrueCodeExample.Common.Exceptions;

namespace TrueCodeExample.Common.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private const string ProblemJsonContentType = "application/problem+json";

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DomainException domainException)
        {
            await WriteProblemAsync(context, domainException.StatusCode, domainException.Message, domainException);
        }
        catch (Exception exception)
        {
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.", exception);
        }
    }

    private async Task WriteProblemAsync(HttpContext context, HttpStatusCode statusCode, string detail, Exception exception)
    {
        if ((int)statusCode >= 500)
        {
            logger.LogError(exception, "Request failed with status {StatusCode}", (int)statusCode);
        }
        else
        {
            logger.LogWarning("Request rejected with status {StatusCode}: {Message}", (int)statusCode, exception.Message);
        }

        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.Clear();
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = ProblemJsonContentType;

        var problem = new ProblemResponse
        {
            Status = (int)statusCode,
            Title = ReasonPhrase(statusCode),
            Detail = detail,
            Errors = (exception as ValidationException)?.Errors
        };

        await context.Response.WriteAsJsonAsync(problem, (JsonSerializerOptions?)null, ProblemJsonContentType);
    }

    private static string ReasonPhrase(HttpStatusCode statusCode) => statusCode switch
    {
        HttpStatusCode.BadRequest => "Bad Request",
        HttpStatusCode.Unauthorized => "Unauthorized",
        HttpStatusCode.Forbidden => "Forbidden",
        HttpStatusCode.NotFound => "Not Found",
        HttpStatusCode.Conflict => "Conflict",
        _ => "Internal Server Error"
    };
}

public sealed class ProblemResponse
{
    public int Status { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Detail { get; init; } = string.Empty;
    public IReadOnlyDictionary<string, string[]>? Errors { get; init; }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseTrueCodeExceptionHandling(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionHandlingMiddleware>();
}

using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using news.feed.models.Exceptions;
using news.feed.models.Exceptions.Auth;

namespace news.feed.Services.Exceptions;

// ReSharper disable once RedundantSwitchExpressionArms
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        _logger.LogError(exception, exception.Message);

        var statusCode = DetermineErrorCode(exception);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = "An error occurred while processing request",
            Detail = exception.Message,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, ct);

        return true;
    }

    private static int DetermineErrorCode(Exception exception)
    {
        var statusCode = exception switch
        {
            // HTTP 400
            ValidationFailedException => HttpStatusCode.BadRequest,
            FailedToAuthenticateException => HttpStatusCode.BadRequest,

            // HTTP 404
            DataNotFoundException => HttpStatusCode.NotFound,

            // HTTP 500
            FailedToCreateSecretException => HttpStatusCode.InternalServerError,
            FailToModifyDataException => HttpStatusCode.InternalServerError,
            _ => HttpStatusCode.InternalServerError
        };

        return (int)statusCode;
    }
}
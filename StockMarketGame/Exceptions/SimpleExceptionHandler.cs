using Microsoft.AspNetCore.Diagnostics;
using StockMarketGame.Common;

namespace StockMarketGame.Exceptions;

public class SimpleExceptionHandler(ILogger<SimpleExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            "Error Message: {exceptionMessage}, Time of occurrence {time}",
            exception.Message, DateTime.UtcNow);

        (int statusCode, string errorCode) = exception switch
        {
            DuplicateKeyException => (StatusCodes.Status409Conflict, "DUPLICATE_KEY"),
            ArgumentException => (StatusCodes.Status400BadRequest, "INVALID_ARGUMENT"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "NOT_FOUND"),
            _ => (StatusCodes.Status500InternalServerError, "INTERNAL_ERROR")
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(
            ErrorResponse.FromException(exception, errorCode), 
            cancellationToken
        );
        
        return true;
    }
}
using ProjectTaskManagement.Application.Common.Exceptions;
using ProjectTaskManagement.Application.Common.Models;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace ProjectTaskManagement.Api.Extensions;

public class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case ValidationException validationException:
                await HandleValidationException(httpContext, validationException);
                return true;

            case NotFoundException notFoundException:
                await HandleNotFoundException(httpContext, notFoundException);
                return true;

            case UnauthorizedAccessException unauthorizedException:
                await HandleUnauthorizedAccessException(httpContext, unauthorizedException);
                return true;

            default:
                return false;
        }
    }

    private static async Task HandleValidationException(HttpContext httpContext, ValidationException exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(new DataResponse<IDictionary<string, string[]>>
        {
            IsSuccess = false,
            StatusCode = HttpStatusCode.BadRequest,
            ResponseMessage = "Validation failed.",
            ResponseData = exception.Errors
        });
    }

    private static async Task HandleNotFoundException(HttpContext httpContext, NotFoundException exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        await httpContext.Response.WriteAsJsonAsync(new DataResponse<string>
        {
            IsSuccess = false,
            StatusCode = HttpStatusCode.NotFound,
            ResponseMessage = exception.Message,
            ResponseData = exception.Message
        });
    }

    private static async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await httpContext.Response.WriteAsJsonAsync(new DataResponse<string>
        {
            IsSuccess = false,
            StatusCode = HttpStatusCode.Unauthorized,
            ResponseMessage = exception.Message,
            ResponseData = exception.Message
        });
    }
}

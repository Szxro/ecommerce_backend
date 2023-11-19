using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Web_Api.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);

        } catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        (int httpStatusCode,string message) exceptionResult = MapException(exception);

        context.Response.StatusCode = exceptionResult.httpStatusCode;

        context.Response.ContentType = "application/json"; // important to write the content correctly

        await context.Response.WriteAsync(exceptionResult.message);
    }

    private (int httpStatusCode, string message) MapException(Exception exception) => exception switch 
    {
        PasswordException passwordException => ((int)HttpStatusCode.BadRequest, JsonSerializer.Serialize(new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "PasswordException Happen",
            Detail = passwordException.Message
        })),
        ValidationsException validationsException => ((int)HttpStatusCode.BadRequest,JsonSerializer.Serialize(
            new ValidationProblemDetails(validationsException._Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "Validation Exception Happen"
        })),
        NotFoundException notFoundException =>((int)HttpStatusCode.BadRequest, JsonSerializer.Serialize(new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "NotFoundException Happen",
            Detail = notFoundException.Message
        })), 
        _ => ((int)HttpStatusCode.InternalServerError, JsonSerializer.Serialize(new ProblemDetails()
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Title = "Internal Server Error",
            Detail = exception.Message
        }))
    };
}

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

    private async Task HandleExceptionAsync(HttpContext context,Exception exception)
    {
        string result = GetErrorMessage(exception);

        context.Response.StatusCode = GetHttpStatusCode(exception);

        context.Response.ContentType = "application/json"; // important to write the content correctly

        await context.Response.WriteAsync(result);
    }

    private int GetHttpStatusCode(Exception exception) => exception switch
    {
        PasswordException passwordException => (int)HttpStatusCode.BadRequest,
        ValidationsException validationsException=> (int)HttpStatusCode.BadRequest,
        NotFoundException notFoundException => (int)HttpStatusCode.NotFound,
        _ => (int)HttpStatusCode.InternalServerError
    };

    private string GetErrorMessage(Exception exception) => exception switch
    {
        PasswordException passwordException => JsonSerializer.Serialize(new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = $"{exception.GetType().Name} Happen",
            Detail = passwordException.Message
        }),
        ValidationsException validationsException => JsonSerializer.Serialize(new ValidationProblemDetails(validationsException._Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "Validation Exception Happen"
        }),
        NotFoundException notFoundException => JsonSerializer.Serialize(new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = $"{exception.GetType().Name} Happen",
            Detail = notFoundException.Message
        }),
        _ => JsonSerializer.Serialize(new ProblemDetails()
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Title = "Internal Server Error",
            Detail = exception.Message
        })
    };
}

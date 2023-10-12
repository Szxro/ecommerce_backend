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
        string result = string.Empty;

        switch (exception)
        {
            case PasswordException passwordException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new ProblemDetails()
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Title = $"{exception.GetType().Name} Happen",
                    Detail = passwordException.Message
                });
                break;
            case ValidationsException validationsException:
                context.Response.StatusCode= (int)HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new ValidationProblemDetails(validationsException._Errors)
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Title = "Validation Exception Happen"
                });
                break;
            case NotFoundException notFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new ProblemDetails()
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Title = $"{exception.GetType().Name} Happen",
                    Detail = notFoundException.Message
                });
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result = JsonSerializer.Serialize(new ProblemDetails()
                {
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                    Title = "Internal Server Error",
                    Detail = exception.Message
                });
                break;
        }

        context.Response.ContentType = "application/json"; // important to write the content correctly

        await context.Response.WriteAsync(result);
    }
}

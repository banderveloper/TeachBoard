using System.Net;
using TeachBoard.IdentityService.Application.Exceptions;

namespace TeachBoard.IdentityService.WebApi.Middleware;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next)
        => _next = next;
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        IApiException? result = null;

        switch (exception)
        {
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                result = notFoundException;
                break;
            case AlreadyExistsException alreadyExistsException:
                statusCode = HttpStatusCode.BadRequest;
                result = alreadyExistsException;
                break;
            case RefreshTokenException refreshTokenException:
                statusCode = HttpStatusCode.NotAcceptable;
                result = refreshTokenException;
                break;
            case WrongPasswordException wrongPasswordException:
                statusCode = HttpStatusCode.Forbidden;
                result = wrongPasswordException;
                break;
            default:
                Console.WriteLine(exception);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return result is not null
            ? context.Response.WriteAsJsonAsync(result)
            : context.Response.WriteAsJsonAsync(new
            {
                error = "unknown_error",
                errorDescription = "Unknown server error"
            });
    }
}

public static class CustomExceptionHandlerMiddlewareExtension
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}
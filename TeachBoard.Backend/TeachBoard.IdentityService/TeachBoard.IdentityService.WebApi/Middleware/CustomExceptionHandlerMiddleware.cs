using System.Net;
using System.Reflection.Metadata.Ecma335;
using TeachBoard.IdentityService.Application.Exceptions;

namespace TeachBoard.IdentityService.WebApi.Middleware;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

    public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        switch (exception)
        {
            case IExpectedApiException expectedApiException:
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                await context.Response.WriteAsJsonAsync(expectedApiException);
                break;

            case INotAcceptableRequestException notAcceptableRequestException:
                context.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                await context.Response.WriteAsJsonAsync(notAcceptableRequestException);
                break;

            default:
                _logger.LogError(exception.ToString());
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "unknown_error",
                    errorDescription = "Unknown server error"
                });
                break;
        }
    }
}

public static class CustomExceptionHandlerMiddlewareExtension
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}
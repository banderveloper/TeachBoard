using System.Net;
using TeachBoard.Gateway.Application.Exceptions;
using TeachBoard.Gateway.Application.Extensions;
using CookieException = TeachBoard.Gateway.Application.Exceptions.CookieException;

namespace TeachBoard.Gateway.WebApi.Middleware;

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
        var statusCode = HttpStatusCode.InternalServerError;
        IApiException? responseBody = null;

        switch (exception)
        {
            // Exception from microservices
            case Refit.ApiException refitApiException:
                statusCode = refitApiException.StatusCode;

                if (statusCode == HttpStatusCode.UnprocessableEntity)
                {
                    context.Response.StatusCode = (int)statusCode;
                    await context.Response.WriteAsJsonAsync(refitApiException.ToValidationResultDictionary());
                    return;
                }

                responseBody = await refitApiException.ToServiceException();
                break;

            // Needed microservice is down
            case HttpRequestException:
                statusCode = HttpStatusCode.ServiceUnavailable;
                break;

            case JwtPayloadException jwtPayloadException:
                statusCode = HttpStatusCode.NotAcceptable;
                responseBody = jwtPayloadException;
                break;

            case CookieException cookieException:
                statusCode = HttpStatusCode.NotAcceptable;
                responseBody = cookieException;
                break;

            default:
                _logger.LogError(exception.ToString());
                break;
        }

        if (responseBody is not null)
            await WriteResponseAsync(context, responseBody, statusCode);
        else
            await WriteResponseAsync(context, new { message = "An error occurred." }, statusCode);
    }

    private async Task WriteResponseAsync<T>(HttpContext context, T responseBody, HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsJsonAsync(responseBody);
    }
}

public static class CustomExceptionHandlerMiddlewareExtension
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}
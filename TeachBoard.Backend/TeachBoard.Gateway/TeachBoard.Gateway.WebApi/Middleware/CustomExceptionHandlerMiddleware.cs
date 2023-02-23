using System.Net;
using TeachBoard.Gateway.Application.Exceptions;
using TeachBoard.Gateway.Application.Extensions;

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
        object? responseBody = null;

        switch (exception)
        {
            // Exception from microservices
            case Refit.ApiException refitApiException:
                statusCode = refitApiException.StatusCode;

                responseBody =
                    statusCode == HttpStatusCode.UnprocessableEntity
                        ? await refitApiException.ToValidationResultDictionary()
                        : await refitApiException.ToServiceException();

                break;

            // Needed microservice is down
            case HttpRequestException:
                statusCode = HttpStatusCode.ServiceUnavailable;
                break;

            case JwtPayloadException jwtPayloadException:
                statusCode = HttpStatusCode.NotAcceptable;
                responseBody = jwtPayloadException;
                break;

            default:
                _logger.LogError(exception.Message);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        if (responseBody is not null)
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
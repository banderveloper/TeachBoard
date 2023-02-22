using System.Net;
using TeachBoard.Gateway.Application.Exceptions;
using TeachBoard.Gateway.Application.Extensions;

namespace TeachBoard.Gateway.WebApi.Middleware;

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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        IApiException responseBody = null;

        switch (exception)
        {
            // Exception from microservices
            case Refit.ApiException refitApiException:
                statusCode = refitApiException.StatusCode;
                responseBody = await refitApiException.ToServiceException();
                break;

            // Needed microservice is down
            case HttpRequestException:
                statusCode = HttpStatusCode.ServiceUnavailable;
                break;

            case JwtPayloadException jwtPayloadException:
                statusCode = HttpStatusCode.Unauthorized;
                responseBody = jwtPayloadException;
                break;

            default:
                Console.WriteLine(exception);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        if (responseBody is not null)
            context.Response.WriteAsJsonAsync(responseBody);
    }
}

public static class CustomExceptionHandlerMiddlewareExtension
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}
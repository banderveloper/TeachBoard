using System.Net;

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
        Dictionary<string, object>? responseBody = null;

        switch (exception)
        {
            // Exception from microservices
            case Refit.ApiException refitApiException:
                statusCode = refitApiException.StatusCode;
                responseBody = await refitApiException.GetContentAsAsync<Dictionary<string, object>>();
                break;

            // Needed microservice is down
            case HttpRequestException:
                statusCode = HttpStatusCode.ServiceUnavailable;
                responseBody = new Dictionary<string, object>
                {
                    { "error", "service_unavailable" },
                    { "errorDescription", "One of the needed services is unavailable now" }
                };
                break;

            default:
                Console.WriteLine(exception);
                break;
        }

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
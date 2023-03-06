using System.Net;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.Gateway.Application;
using TeachBoard.Gateway.Application.Exceptions;
using TeachBoard.Gateway.WebApi.ActionResults;

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
        context.Response.ContentType = "application/json";
        var response = new WebApiResult();

        switch (exception)
        {
            // case IExpectedApiException expectedApiException:
            //     response.StatusCode = HttpStatusCode.OK;
            //     response.Error = new
            //     {
            //         expectedApiException.ErrorCode,
            //         expectedApiException.ReasonField,
            //         message = expectedApiException.PublicErrorMessage
            //     };
            //     break;

            case INotAcceptableRequestException notAcceptableRequestException:
                response.StatusCode = HttpStatusCode.NotAcceptable;
                response.Error = new
                {
                    notAcceptableRequestException.ErrorCode
                };
                break;
            
            // Needed microservice is down
            case HttpRequestException:
                response.StatusCode = HttpStatusCode.ServiceUnavailable;
                response.Error = new
                {
                    errorCode = ErrorCode.NeededServiceUnavailable
                };
                break;

            default:
                response.Error = new { error = ErrorCode.Unknown };
                response.StatusCode = HttpStatusCode.InternalServerError;

                _logger.LogError(exception.ToString());
                break;
        }

        await response.ExecuteResultAsync(new ActionContext
        {
            HttpContext = context
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
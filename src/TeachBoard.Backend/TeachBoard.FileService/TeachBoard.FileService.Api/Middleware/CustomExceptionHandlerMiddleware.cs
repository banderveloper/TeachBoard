using System.Net;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.FileService.Application;
using TeachBoard.FileService.Application.Exceptions;

namespace TeachBoard.FileService.Api.Middleware;

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
            case IExpectedApiException expectedApiException:
                response.StatusCode = HttpStatusCode.OK;
                response.Error = new
                {
                    expectedApiException.ErrorCode,
                    expectedApiException.ReasonField,
                    message = expectedApiException.PublicErrorMessage
                };
                break;

            case IBadRequestApiException badRequestApiException:
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Error = new
                {
                    badRequestApiException.ErrorCode,
                    badRequestApiException.ReasonField,
                    message = badRequestApiException.PublicErrorMessage,
                };
                break;

            case IHostingErrorException hostingErrorException:
                response.StatusCode = HttpStatusCode.BadGateway;
                response.Error = new
                {
                    hostingErrorException.ErrorCode,
                    message = hostingErrorException.PublicErrorMessage
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
using System.Net;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.WebApi.ActionResults;

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
        var response = new WebApiResult();

        switch (exception)
        {
            case IExpectedApiException expectedApiException:
                response.Error = new
                {
                    expectedApiException.ErrorCode,
                    expectedApiException.ReasonField,
                    expectedApiException.PublicErrorMessage
                };
                break;

            case INotAcceptableRequestException notAcceptableRequestException:
                response.StatusCode = HttpStatusCode.NotAcceptable;
                response.Error = new
                {
                    notAcceptableRequestException.ErrorCode
                };
                break;

            default:
                response.Error = new { error = "unknown_error" };
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
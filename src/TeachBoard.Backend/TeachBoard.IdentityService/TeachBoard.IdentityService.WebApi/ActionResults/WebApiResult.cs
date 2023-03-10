using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace TeachBoard.IdentityService.WebApi.ActionResults;

/// <summary>
/// Unified response model with nullable data and error, returned in each point
/// </summary>
public class WebApiResult : ActionResult
{
    public object? Data { get; set; }
    public object? Error { get; set; }
    [JsonIgnore] public HttpStatusCode StatusCode { get; set; }

    public WebApiResult(object? data = null, object? error = null, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        Data = data;
        Error = error;
        StatusCode = statusCode;
    }
    
    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;

        response.ContentType = "application/json";
        response.StatusCode = (int)StatusCode;

        // if (Error is null) Data ??= new object();

        await response.WriteAsJsonAsync(this);
    }
}
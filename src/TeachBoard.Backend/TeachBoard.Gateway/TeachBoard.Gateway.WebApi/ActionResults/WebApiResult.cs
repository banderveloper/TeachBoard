﻿using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Refit;
using TeachBoard.Gateway.Application.Refit;

namespace TeachBoard.Gateway.WebApi.ActionResults;

/// <summary>
/// Unified response model with nullable data and error, returned in each point
/// </summary>
public class WebApiResult : ActionResult
{
    [JsonPropertyName("data")] public object? Data { get; set; }
    [JsonPropertyName("error")] public object? Error { get; set; }
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
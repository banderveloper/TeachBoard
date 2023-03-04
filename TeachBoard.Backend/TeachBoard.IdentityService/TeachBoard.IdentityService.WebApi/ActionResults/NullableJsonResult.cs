using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace TeachBoard.IdentityService.WebApi.ActionResults;

/// <summary>
/// 200-OK response, that transforms NULL response to empty json {} 
/// </summary>
public class NullableJsonResult : ActionResult
{
    private readonly object? _data;

    public NullableJsonResult(object? data)
    {
        _data = data;
    }

    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;

        response.ContentType = "application/json";
        response.StatusCode = 200;

        if (_data is not null)
            await response.WriteAsJsonAsync(_data);
        else
            await response.WriteAsync("{}");
    }
}
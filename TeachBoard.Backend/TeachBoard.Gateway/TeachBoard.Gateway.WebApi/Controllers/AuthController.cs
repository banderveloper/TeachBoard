using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.Gateway.Application.Refit.Clients;
using TeachBoard.Gateway.Application.Refit.RequestModels.Identity;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Identity;
using TeachBoard.Gateway.Application.Services;
using TeachBoard.Gateway.WebApi.ActionResults;

namespace TeachBoard.Gateway.WebApi.Controllers;

[Route("api/auth")]
[AllowAnonymous]
public class AuthController : BaseController
{
    private readonly IIdentityClient _identityClient;
    private readonly CookieService _cookieService;

    public AuthController(IIdentityClient identityClient, CookieService cookieService)
    {
        _identityClient = identityClient;
        _cookieService = cookieService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AccessTokenResponseModel>> Login([FromBody] LoginRequestModel model)
    {
        var identityServiceResponse = await _identityClient.Login(model);

        // Transfer cookies from microservice response to headers for client
        _cookieService.TransferCookies(
            sourceHeaders: identityServiceResponse.Headers,
            destinationCookies: Response.Cookies
        );

        // auth model with access token and expire time
        return new WebApiResult(
            data: identityServiceResponse.Content?.Data,
            error: identityServiceResponse.Content?.Error,
            statusCode: identityServiceResponse.StatusCode
        );
    }

    // [HttpPut("refresh")]
    // public async Task<ActionResult<AuthTokenResponseModel>> Refresh()
    // {
    //     if (!Request.Cookies.TryGetValue("TeachBoard-Refresh-Token", out var refreshTokenFromCookie))
    //     {
    //         throw new CookieException
    //         {
    //             Error = "cookie_not_found",
    //             ErrorDescription = "Cookie with refresh token not found"
    //         };
    //     }
    //
    //     // Send refresh query to identity microservice including cookie
    //     var identityServiceResponse =
    //         await _identityClient.Refresh($"TeachBoard-Refresh-Token={refreshTokenFromCookie}");
    //
    //     // if error from microservice
    //     if (!identityServiceResponse.IsSuccessStatusCode)
    //         throw identityServiceResponse.Error;
    //
    //     // Transfer cookies from microservice response to headers for client
    //     _cookieService.TransferCookies(
    //         sourceHeaders: identityServiceResponse.Headers,
    //         destinationCookies: Response.Cookies
    //     );
    //
    //     // authmodel with access token and expire time
    //     return Ok(identityServiceResponse.Content);
    // }
    //
    // [HttpDelete("logout")]
    // public async Task<IActionResult> Logout()
    // {
    //     if (!Request.Cookies.TryGetValue("TeachBoard-Refresh-Token", out var refreshTokenFromCookie))
    //     {
    //         throw new CookieException
    //         {
    //             Error = "cookie_not_found",
    //             ErrorDescription = "Cookie with refresh token not found"
    //         };
    //     }
    //
    //     // Send refresh query to identity microservice including cookie
    //     var logoutResponse = await _identityClient.Logout($"TeachBoard-Refresh-Token={refreshTokenFromCookie}");
    //
    //     // if error from microservice
    //     if (!logoutResponse.IsSuccessStatusCode)
    //         throw logoutResponse.Error;
    //
    //     // Transfer cookies from microservice response to headers for client
    //     _cookieService.TransferCookies(
    //         sourceHeaders: logoutResponse.Headers,
    //         destinationCookies: Response.Cookies
    //     );
    //
    //     // authmodel with access token and expire time
    //     return Ok();
    // }
}
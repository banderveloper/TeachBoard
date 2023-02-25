using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.Gateway.Application.Exceptions;
using TeachBoard.Gateway.Application.Models.Identity.Request;
using TeachBoard.Gateway.Application.Models.Identity.Response;
using TeachBoard.Gateway.Application.RefitClients;
using TeachBoard.Gateway.Application.Services;

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
    public async Task<ActionResult<AuthTokenResponseModel>> Login([FromBody] LoginRequestModel model)
    {
        var identityServiceResponse = await _identityClient.Login(model);

        // if error from microservice
        if (!identityServiceResponse.IsSuccessStatusCode)
            throw identityServiceResponse.Error;

        // Transfer cookies from microservice response to headers for client
        _cookieService.TransferCookies(
            sourceHeaders: identityServiceResponse.Headers,
            destinationCookies: Response.Cookies
        );

        // authmodel with access token and expire time
        return Ok(identityServiceResponse.Content);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthTokenResponseModel>> Refresh()
    {
        if (!Request.Cookies.TryGetValue("TeachBoard-Refresh-Token", out var refreshTokenFromCookie))
        {
            throw new CookieException
            {
                Error = "cookie_not_found",
                ErrorDescription = "Cookie with refresh token not found"
            };
        }
        
        // Send refresh query to identity microservice including cookie
        var identityServiceResponse =
            await _identityClient.Refresh($"TeachBoard-Refresh-Token={refreshTokenFromCookie}");

        // if error from microservice
        if (!identityServiceResponse.IsSuccessStatusCode)
            throw identityServiceResponse.Error;

        // Transfer cookies from microservice response to headers for client
        _cookieService.TransferCookies(
            sourceHeaders: identityServiceResponse.Headers,
            destinationCookies: Response.Cookies
        );

        // authmodel with access token and expire time
        return Ok(identityServiceResponse.Content);
    }
}
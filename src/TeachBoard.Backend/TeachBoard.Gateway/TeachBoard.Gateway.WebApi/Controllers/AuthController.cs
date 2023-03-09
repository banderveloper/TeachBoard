using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.Gateway.Application;
using TeachBoard.Gateway.Application.Exceptions;
using TeachBoard.Gateway.Application.Refit.Clients;
using TeachBoard.Gateway.Application.Refit.RequestModels.Identity;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Identity;
using TeachBoard.Gateway.Application.Services;
using TeachBoard.Gateway.Application.Validation;
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

    /// <summary>
    /// Sign in using username and password
    /// </summary>
    /// 
    /// <param name="model">Username and password</param>
    ///
    /// <response code="200">Success / user_not_found / user_password_incorrect</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="503">Authentication service is unavailable now</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AccessTokenModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<AccessTokenModel>> Login([FromBody] LoginRequestModel model)
    {
        var identityServiceResponse = await _identityClient.Login(model);
        var accessTokenModel = identityServiceResponse.Content?.Data;
        
        // Transfer cookies from microservice response to headers for client
        _cookieService.TransferCookies(
            sourceHeaders: identityServiceResponse.Headers,
            destinationCookies: Response.Cookies
        );

        // auth model with access token and expire time
        return new WebApiResult(accessTokenModel);
    }

    /// <summary>
    /// Refresh user session using cookie refresh token 
    /// </summary>
    ///
    /// <remarks>Requires cookie with refresh token</remarks>
    ///
    /// <response code="200">Success / session_not_found</response>
    /// <response code="406">refresh_cookie_not_found</response>
    /// <response code="503">Authentication service is unavailable now</response>
    [HttpPut("refresh")]
    [ProducesResponseType(typeof(AccessTokenModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IBadRequestApiException), StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<AccessTokenModel>> Refresh()
    {
        // Get refresh token from cookies. if not passed - error
        if (!Request.Cookies.TryGetValue("TeachBoard-Refresh-Token", out var refreshTokenFromCookie))
            throw new NotAcceptableRequestException { ErrorCode = ErrorCode.RefreshCookieNotFound };
                
        // Send refresh query to identity microservice including cookie
        var identityServiceResponse =
            await _identityClient.Refresh($"TeachBoard-Refresh-Token={refreshTokenFromCookie}");
        
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
    
    /// <summary>
    /// Logout. End session and destroy refresh token
    /// </summary>
    ///
    /// <remarks>Requires cookie with refresh token</remarks>
    ///
    /// <response code="200">Success / session_not_found</response>
    /// <response code="406">refresh_cookie_not_found</response>
    /// <response code="503">Authentication service is unavailable now</response>
    [HttpDelete("logout")]
    [ProducesResponseType(typeof(AccessTokenModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IBadRequestApiException), StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Logout()
    {
        // Get refresh token from cookies. if not passed - error
        if (!Request.Cookies.TryGetValue("TeachBoard-Refresh-Token", out var refreshTokenFromCookie))
            throw new NotAcceptableRequestException { ErrorCode = ErrorCode.RefreshCookieNotFound };
    
        // Send refresh query to identity microservice including cookie
        var identityServiceResponse = await _identityClient.Logout($"TeachBoard-Refresh-Token={refreshTokenFromCookie}");
        
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
}
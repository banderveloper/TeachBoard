using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.IdentityService.Application.Configurations;
using TeachBoard.IdentityService.Application.CQRS.Commands;
using TeachBoard.IdentityService.Application.CQRS.Queries;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Application.Extensions;
using TeachBoard.IdentityService.Application.Services;
using TeachBoard.IdentityService.WebApi.ActionResults;
using TeachBoard.IdentityService.WebApi.Models.Auth;
using TeachBoard.IdentityService.WebApi.Validation;

namespace TeachBoard.IdentityService.WebApi.Controllers;

[ApiController]
[Route("auth")]
[Produces("application/json")]

public class AuthController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    private readonly CookieProvider _cookieProvider;
    private readonly JwtProvider _jwtProvider;

    private readonly JwtConfiguration _jwtConfiguration;

    public AuthController(IMapper mapper, IMediator mediator, CookieProvider cookieProvider, JwtProvider jwtProvider,
        JwtConfiguration jwtConfiguration)
    {
        _mapper = mapper;
        _mediator = mediator;
        _cookieProvider = cookieProvider;
        _jwtProvider = jwtProvider;
        _jwtConfiguration = jwtConfiguration;
    }

    /// <summary>
    /// User login and session creation
    /// </summary>
    ///
    /// <remarks>With access token in body, returns http-only cookie (TeachBoard-Refresh-Token) with refresh token</remarks>
    /// 
    /// <param name="requestModel">User credentials login model</param>
    /// 
    /// <response code="200">Successful login</response>
    /// <response code="403">Incorrect password (wrong_password)</response>
    /// <response code="404">User with given username not found (user_not_found)</response>
    /// <response code="422">Invalid model</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<AccessTokenResponseModel>> Login([FromBody] LoginRequestModel requestModel)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        // get user by username and password
        var getUserQuery = _mapper.Map<GetUserByCredentialsQuery>(requestModel);
        var user = await _mediator.Send(getUserQuery);

        // create/update refresh session by user id ang get new refresh token
        var setSessionCommand = new SetRefreshSessionCommand { UserId = user.Id };
        var session = await _mediator.Send(setSessionCommand);

        // add refresh token to http-only cookie
        _cookieProvider.AddRefreshCookieToResponse(HttpContext.Response, session.RefreshToken);

        // generate access token for user
        var accessToken = _jwtProvider.GenerateUserJwt(user);

        return new WebApiResult(new AccessTokenResponseModel
        {
            AccessToken = accessToken,
            Expires = DateTime.Now.AddMinutes(_jwtConfiguration.MinutesToExpiration).ToUnixTimestamp()
        });
    }


    /// <summary>
    /// Session and tokens update
    /// </summary>
    /// 
    /// <remarks>
    /// Takes TeachBoard-Refresh-Token, and if session exists - update it and return new tokens
    /// </remarks>
    /// 
    /// <param>Refresh token in cookie TeachBoard-Refresh-Token</param>
    /// <response code="200">Successful session update</response>
    /// <response code="404">Session connected to given refresh token not found (session_not_found) / User connected to session not found (user_not_found)</response>
    /// <response code="406">Did not pass refresh token at TeachBoard-Refresh-Token (refresh_token_not_found)</response>
    [HttpPut("refresh")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status406NotAcceptable)]
    public async Task<ActionResult<AccessTokenResponseModel>> Refresh()
    {
        // get refresh token from http-only cookie
        var refreshToken = _cookieProvider.GetRefreshTokenFromCookie(Request);

        // update refresh session. If request refresh token incorrect - exception
        // if everything ok - get all session, including user id
        var updateSessionCommand = new UpdateRefreshSessionCommand { RefreshToken = refreshToken };
        var refreshSession = await _mediator.Send(updateSessionCommand);

        // get user of this session
        var getUserQuery = new GetUserByIdQuery { UserId = refreshSession.UserId };
        var user = await _mediator.Send(getUserQuery);

        // add refresh token to http-only cookie
        _cookieProvider.AddRefreshCookieToResponse(HttpContext.Response, refreshSession.RefreshToken);

        // generation new access token and return it
        var accessToken = _jwtProvider.GenerateUserJwt(user);

        return new WebApiResult(new AccessTokenResponseModel
        {
            AccessToken = accessToken,
            Expires = DateTime.Now.AddMinutes(_jwtConfiguration.MinutesToExpiration).ToUnixTimestamp()
        });
    }

    /// <summary>
    /// Logout and session stop
    /// </summary>
    /// 
    /// <remarks>
    /// Takes TeachBoard-Refresh-Token, and if session exists - delete binded session
    /// </remarks>
    /// 
    /// <param>Refresh token in cookie TeachBoard-Refresh-Token</param>
    /// <response code="200">Successful logout</response>
    /// <response code="404">Session connected to given refresh token not found (session_not_found)</response>
    /// <response code="406">Did not pass refresh token at TeachBoard-Refresh-Token (refresh_token_not_found)</response>
    [HttpDelete("logout")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status406NotAcceptable)]
    public async Task<IActionResult> Logout()
    {
        // get refresh token from http-only cookie
        var refreshToken = _cookieProvider.GetRefreshTokenFromCookie(Request);

        // delete session by refresh token from db
        var deleteCommand = new DeleteRefreshSessionByTokenCommand { RefreshToken = refreshToken };
        await _mediator.Send(deleteCommand);

        // delete cookie
        Response.Cookies.Delete("TeachBoard-Refresh-Token");

        return new WebApiResult();
    }
}
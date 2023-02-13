using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.IdentityService.Application.Configurations;
using TeachBoard.IdentityService.Application.CQRS.Commands.SetRefreshSession;
using TeachBoard.IdentityService.Application.CQRS.Queries.GetUserByCredentials;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Application.Services;
using TeachBoard.IdentityService.WebApi.Models.Auth;
using TeachBoard.IdentityService.WebApi.Models.Validation;

namespace TeachBoard.IdentityService.WebApi.Controllers;

[ApiController]
[Route("auth")]
[Produces("application/json")]
[ValidateModel]
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
    [ProducesResponseType(typeof(void),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginRequestModel requestModel)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        // get user by username and password
        var getUserQuery = _mapper.Map<GetUserByCredentialsQuery>(requestModel);
        var user = await _mediator.Send(getUserQuery);
        
        // create/update refresh session by user id ang get new refresh token
        var setSessionCommand = new SetRefreshSessionCommand {UserId = user.Id};
        var refreshToken = await _mediator.Send(setSessionCommand);
        
        // add refresh token to http-only cookie
        _cookieProvider.AddRefreshCookieToResponse(HttpContext.Response, refreshToken);
        
        // generate access token for user
        var accessToken = _jwtProvider.GenerateUserJwt(user);
        
        return Ok(new LoginResponseModel
        {
            AccessToken = accessToken,
            ExpiresAt = DateTime.Now.AddMinutes(_jwtConfiguration.MinutesToExpiration)
        });
    }
}
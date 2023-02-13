using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.IdentityService.Application.Configurations;
using TeachBoard.IdentityService.Application.CQRS.Commands.SetRefreshSession;
using TeachBoard.IdentityService.Application.CQRS.Queries.GetUserByCredentials;
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
    /// Логин пользователя и создании сессии. Получение JWT-токенов доступа и рефреша.
    /// </summary>
    /// 
    /// <remarks>
    /// Указывается ИЛИ телефон, ИЛИ почта. Если и то и другое будет пустым - ответ 400
    /// </remarks>
    /// 
    /// <param name="requestModel">Модель входа в аккаунт</param>
    /// <returns>JWT access token в теле, refresh token в http-only cookie (X-Refresh-Token) при успехе. Модель ошибки при неудаче</returns>
    /// 
    /// <response code="200">Успешный вход</response>
    /// <response code="400">И телефон и почта не указаны</response>
    /// <response code="404">Пользователь с такими учетными данными не найден.</response>
    /// <response code="422">Невалидная модель (например не указаны обязательные поля)</response>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
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
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.Gateway.Application;
using TeachBoard.Gateway.Application.Exceptions;

namespace TeachBoard.Gateway.WebApi.Controllers;

[ApiController]
[Produces("application/json")]
public class BaseController : ControllerBase
{
    // User id from jwt token
    internal int UserId => User.Identity.IsAuthenticated
        ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)
        : throw new NotAcceptableRequestException { ErrorCode = ErrorCode.JwtUserIdNotFound };
}
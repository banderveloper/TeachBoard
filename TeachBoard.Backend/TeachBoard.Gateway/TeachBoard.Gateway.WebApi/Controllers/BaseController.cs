using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.Gateway.Application.Exceptions;
using TeachBoard.Gateway.WebApi.Validation;

namespace TeachBoard.Gateway.WebApi.Controllers;

[ApiController]
[ValidateModel]
[Produces("application/json")]
public class BaseController : ControllerBase
{
    // User id from jwt token
    internal string UserId => User.Identity.IsAuthenticated
        ? User.FindFirst(ClaimTypes.NameIdentifier).Value
        : throw new JwtPayloadException
        {
            Error = "jwt_user_id_not_found",
            ErrorDescription = "No user id found at given jwt-token",
            ReasonField = "id"
        };
}
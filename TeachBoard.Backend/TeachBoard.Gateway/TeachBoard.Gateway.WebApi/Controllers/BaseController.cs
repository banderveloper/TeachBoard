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
    internal int UserId => User.Identity.IsAuthenticated
        ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)
        : throw new JwtPayloadException
        {
            Error = "jwt_user_id_not_found",
            ErrorDescription = "No user id found at given jwt-token",
            ReasonField = "id"
        };
}
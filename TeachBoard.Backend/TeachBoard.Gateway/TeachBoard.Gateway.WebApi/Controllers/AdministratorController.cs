using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.Gateway.Application.Exceptions;
using TeachBoard.Gateway.Application.Models.Identity.Request;
using TeachBoard.Gateway.Application.Models.Identity.Response;
using TeachBoard.Gateway.Application.RefitClients;
using TeachBoard.Gateway.Application.Validation;
using TeachBoard.Gateway.Domain.Enums;

namespace TeachBoard.Gateway.WebApi.Controllers;

[Route("api/administrator")]
// [Authorize(Roles = "Administrator")]
public class AdministratorController : BaseController
{
    private readonly IIdentityClient _identityClient;
    private readonly IMembersClient _membersClient;
    private readonly IEducationClient _educationClient;

    public AdministratorController(IIdentityClient identityClient, IMembersClient membersClient,
        IEducationClient educationClient)
    {
        _identityClient = identityClient;
        _membersClient = membersClient;
        _educationClient = educationClient;
    }

    /// <summary>
    /// Create new pending user
    /// </summary>
    /// 
    /// <param name="model">Create pending user requestModel with personal data of user</param>
    /// <returns>Register code and expiration date</returns>
    ///
    /// <response code="200">Success. Pending user created and register code returned</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Administrator can create only Students(1) and Teachers(2) (set_role_permission_error)</response>
    /// <response code="406">Jwt-token does not contains user id (jwt_user_id_not_found)</response>
    /// <response code="409">Pending user with given phone/email already exists (phone_already_exists / email_already_exists)</response>
    /// <response code="422">Invalid requestModel</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpPost("createPendingUser")]
    [ProducesResponseType(typeof(RegisterCodeResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<RegisterCodeResponseModel>> CreatePendingUser(
        [FromBody] CreatePendingUserRequestModel model)
    {
        // If role not student or teacher, error. Admin can create only teachers and 
        if (model.Role != (int)UserRole.Student && model.Role != (int)UserRole.Teacher)
            throw new PermissionException
            {
                Error = "set_role_permission_error",
                ErrorDescription = "Administrator can set only 'Student' and 'Teacher' role to pending users",
                ReasonField = "role"
            };

        var responseCodeModel = await _identityClient.CreatePendingUser(model);

        return responseCodeModel;
    }
}
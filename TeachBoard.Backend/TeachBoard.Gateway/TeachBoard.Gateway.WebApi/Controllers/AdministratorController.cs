using Microsoft.AspNetCore.Mvc;
using TeachBoard.Gateway.Application;
using TeachBoard.Gateway.Application.Exceptions;
using TeachBoard.Gateway.Application.Refit.Clients;
using TeachBoard.Gateway.Application.Refit.RequestModels.Identity;
using TeachBoard.Gateway.Application.Refit.RequestModels.Members;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Identity;
using TeachBoard.Gateway.Application.Validation;
using TeachBoard.Gateway.WebApi.ActionResults;

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
    /// Create pending user
    /// </summary>
    /// 
    /// <param name="model">New pending user data</param>
    ///
    /// <response code="200">Success / set_role_forbidden / email_already_exists / phoneNumber_already_exists</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Invalid model</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpPost("pending-user")]
    [ProducesResponseType(typeof(RegisterCodeModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<RegisterCodeModel>> CreatePendingUser(
        [FromBody] CreatePendingUserRequestModel model)
    {
        // If role not student or teacher, error. Admin can create only teachers and 
        if (model.Role != UserRole.Student && model.Role != UserRole.Teacher)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.SetRoleForbidden,
                PublicErrorMessage = "Administrator can set only 'Student' and 'Teacher' role to pending users",
                ReasonField = "role"
            };

        var identityResponse = await _identityClient.CreatePendingUser(model);
        var registerCodeModel = identityResponse.Data;

        return new WebApiResult(registerCodeModel);
    }

    /// <summary>
    /// Set student group
    /// </summary>
    /// 
    /// <param name="model">Student id and group id</param>
    ///
    /// <response code="200">Success / student_not_found / group_not_found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Invalid model</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpPut("student-group")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> SetStudentGroup([FromBody] SetStudentGroupRequestModel model)
    {
        await _membersClient.SetStudentGroup(model);
        return new WebApiResult();
    }
}
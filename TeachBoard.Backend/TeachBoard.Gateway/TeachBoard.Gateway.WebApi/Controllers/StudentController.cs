using Microsoft.AspNetCore.Mvc;
using TeachBoard.Gateway.Application.Exceptions;
using TeachBoard.Gateway.Application.Models.Identity.Request;
using TeachBoard.Gateway.Application.Models.Members.Request;
using TeachBoard.Gateway.Application.RefitClients;
using TeachBoard.Gateway.WebApi.Validation;

namespace TeachBoard.Gateway.WebApi.Controllers;

[ApiController]
[Route("api/student")]
[Produces("application/json")]
[ValidateModel]
public class StudentController : ControllerBase
{
    private readonly IIdentityClient _identityClient;
    private readonly IMembersClient _membersClient;

    public StudentController(IIdentityClient identityClient, IMembersClient membersClient)
    {
        _identityClient = identityClient;
        _membersClient = membersClient;
    }

    /// <summary>
    /// Approve pending student
    /// </summary>
    /// 
    /// <param name="model">Approve pending student model with needed register code, new username and password</param>
    /// <returns>None</returns>
    ///
    /// <response code="200">Success. Pending student approved</response>
    /// <response code="401">Unathorized</response>
    /// <response code="404">Pending user with given register code not found (register_code_not_found)</response>
    /// <response code="409">User with given username already exists (username_already_exists)</response>
    /// <response code="410">Pending user expired (pending_user_expired)</response>
    /// <response code="422">Invalid model</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpPost("approvepending")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status410Gone)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ApproveStudent([FromBody] ApprovePendingUserRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);
        
        var user = await _identityClient.ApprovePendingUser(model);
        await _membersClient.CreateStudent(new CreateStudentRequestModel { UserId = user.Id });

        return Ok();
    }
}
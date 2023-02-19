using Microsoft.AspNetCore.Mvc;
using TeachBoard.Gateway.Application.Models.Identity.Request;
using TeachBoard.Gateway.Application.Models.Members.Request;
using TeachBoard.Gateway.Application.RefitClients;

namespace TeachBoard.Gateway.WebApi.Controllers;

[ApiController]
[Route("api/student")]
public class StudentController : ControllerBase
{
    private readonly IIdentityClient _identityClient;
    private readonly IMembersClient _membersClient;

    public StudentController(IIdentityClient identityClient, IMembersClient membersClient)
    {
        _identityClient = identityClient;
        _membersClient = membersClient;
    }

    [HttpPost("approvepending")]
    public async Task<IActionResult> ApproveStudent([FromBody] ApprovePendingUserRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);
        
        var user = await _identityClient.ApprovePendingUser(model);
        await _membersClient.CreateStudent(new CreateStudentRequestModel { UserId = user.Id });

        return Ok();
    }
}
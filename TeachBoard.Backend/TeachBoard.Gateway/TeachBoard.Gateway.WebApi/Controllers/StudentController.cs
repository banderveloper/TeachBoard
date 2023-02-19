using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.Ini;
using TeachBoard.Gateway.Application.Models.Identity;
using TeachBoard.Gateway.Application.Models.Members;
using TeachBoard.Gateway.Application.RefitClients;

namespace TeachBoard.Gateway.WebApi.Controllers;

[ApiController]
[Route("api/student")]
public class StudentController : ControllerBase
{
    private IIdentityClient _identityClient;
    private IMembersClient _membersClient;

    public StudentController(IIdentityClient identityClient, IMembersClient membersClient)
    {
        _identityClient = identityClient;
        _membersClient = membersClient;
    }

    [HttpPost("approvepending")]
    public async Task<IActionResult> ApproveStudent([FromBody] ApprovePendingUserTransferModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        var user = await _identityClient.ApprovePendingUser(model);
        await _membersClient.CreateStudent(new StudentCreateRequestModel { UserId = user.Id });

        return Ok();
    }
}
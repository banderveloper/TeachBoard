using Microsoft.AspNetCore.Mvc;
using TeachBoard.StudentAggregator.Clients;
using TeachBoard.StudentAggregator.Models.Identity;

namespace TeachBoard.StudentAggregator.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    private readonly IIdentityClient _identityClient;

    public HomeController(IIdentityClient identityClient)
    {
        _identityClient = identityClient;
    }

    [HttpGet("approvepending")]
    public async Task<IActionResult> ApprovePendingStudent(ApprovePendingUserModel model)
    {
        return Ok(await _identityClient.GetPendingUserRole(model.RegisterCode));
    }
}
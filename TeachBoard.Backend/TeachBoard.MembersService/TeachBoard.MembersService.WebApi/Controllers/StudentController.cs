using Microsoft.AspNetCore.Mvc;
using TeachBoard.MembersService.Application.Validation;

namespace TeachBoard.MembersService.WebApi.Controllers;

[ValidateModel]
[ApiController]
[Route("members/students")]
[Produces("application/json")]
public class StudentController : ControllerBase
{
    [HttpGet]
    public IActionResult Test()
    {
        return Ok("Hello world");
    }
}
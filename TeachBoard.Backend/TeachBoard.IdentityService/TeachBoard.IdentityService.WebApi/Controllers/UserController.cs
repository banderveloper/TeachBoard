using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.IdentityService.Application.CQRS.Commands.ApprovePendingUser;
using TeachBoard.IdentityService.Application.CQRS.Commands.CreatePendingUser;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.WebApi.Models.User;
using TeachBoard.IdentityService.WebApi.Models.Validation;

namespace TeachBoard.IdentityService.WebApi.Controllers;

[ApiController]
[Route("users")]
[Produces("application/json")]
[ValidateModel]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Create new pending user
    /// </summary>
    /// 
    /// <param name="model">Create pending user model with personal data of user</param>
    /// <returns>Register code and expiration date</returns>
    ///
    /// <response code="200">Success. Pending user created</response>
    /// <response code="401">Unathorized</response>
    /// <response code="422">Invalid model</response>
    [HttpPost("pending/create")]
    [ProducesResponseType(typeof(RegisterCodeModel),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<RegisterCodeModel>> CreatePendingUser([FromBody] CreatePendingUserModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        var command = _mapper.Map<CreatePendingUserCommand>(model);

        // Send command which create pending user and return register code and expiration date
        var registerCodeModel = await _mediator.Send(command);

        return Ok(registerCodeModel);
    }
    
    /// <summary>
    /// Approve pending user
    /// </summary>
    /// 
    /// <param name="model">Approve pending user created by administrator</param>
    ///
    /// <response code="200">Success. User approved</response>
    /// <response code="404">Pending user with given register code not found (register_code_not_found)</response>
    /// <response code="422">Invalid model</response>
    [HttpPost("pending/approve")]
    [ProducesResponseType(typeof(void),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ApprovePendingUser([FromBody] ApprovePendingUserModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        // Create a command for approve and send it
        // If everything is ok - it will be created user, pending user will be deleted
        var approveCommand = _mapper.Map<ApprovePendingUserCommand>(model);
        await _mediator.Send(approveCommand);

        return Ok();
    }
}
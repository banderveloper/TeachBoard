using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.Application.CQRS.Commands.CreatePendingUser;
using TeachBoard.WebApi.Models.Auth;

namespace TeachBoard.WebApi.Controllers;

[ApiController]
[Route("users")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    
    [HttpPost("createpending")]
    public async Task<ActionResult<RegisterCodeModel>> CreatePendingUser([FromBody] CreatePendingUserModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        var command = _mapper.Map<CreatePendingUserCommand>(model);

        // Send command which create pending user and return register code
        var registerCodeModel = await _mediator.Send(command);

        return Ok(registerCodeModel);
    }
}
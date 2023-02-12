using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.Application.CQRS.Commands.CreatePendingUser;
using TeachBoard.WebApi.Models.Auth;

namespace TeachBoard.WebApi.Controllers;

[ApiController]
[Route("auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public AuthController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }


    
}
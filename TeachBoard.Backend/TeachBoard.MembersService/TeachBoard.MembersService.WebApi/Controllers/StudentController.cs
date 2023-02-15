using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.MembersService.Application.Validation;

namespace TeachBoard.MembersService.WebApi.Controllers;

[ValidateModel]
[ApiController]
[Route("members/students")]
[Produces("application/json")]
public class StudentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public StudentController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
}
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.MembersService.Application.Features.Students;
using TeachBoard.MembersService.Application.Validation;
using TeachBoard.MembersService.Domain.Entities;

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

    [HttpGet("getbyid/{id:int}")]
    public async Task<ActionResult<Student>> GetStudentById(int id)
    {
        var query = new GetStudentByIdQuery { StudentId = id };
        var student = await _mediator.Send(query);

        return Ok(student);
    }
}
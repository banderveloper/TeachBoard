using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Features.Teachers;
using TeachBoard.MembersService.Application.Validation;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.WebApi.Models.Teacher;

namespace TeachBoard.MembersService.WebApi.Controllers;

[ValidateModel]
[ApiController]
[Route("members/teachers")]
[Produces("application/json")]
public class TeacherController : ControllerBase
{
    private readonly IMediator _mediator;

    public TeacherController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Get teacher by id
    /// </summary>
    /// <param name="id">Teacher id</param>
    /// <response code="200">Success. Teacher returns.</response>
    /// <response code="404">Teacher with given id not found (teacher_not_found)</response>
    [HttpGet("getbyid/{id:int}")]
    [ProducesResponseType(typeof(Teacher), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Student>> GetStudentById(int id)
    {
        var query = new GetTeacherByIdQuery { TeacherId = id };
        var teacher = await _mediator.Send(query);

        return Ok(teacher);
    }
    
    /// <summary>
    /// Create teacher
    /// </summary>
    /// <param name="model">Teacher creation model</param>
    /// <response code="200">Success. Teacher created</response>
    /// <response code="409">Teacher with given user id already exists (teacher_already_exists)</response>
    [HttpPost("create")]
    [ProducesResponseType(typeof(Teacher), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Teacher>> CreateTeacher([FromBody] CreateTeacherRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        var command = new CreateTeacherCommand { UserId = model.UserId };
        var teacher = await _mediator.Send(command);

        return teacher;
    }
}
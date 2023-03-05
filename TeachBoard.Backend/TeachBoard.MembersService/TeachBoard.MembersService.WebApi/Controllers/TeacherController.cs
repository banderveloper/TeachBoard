using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Features.Teachers;
using TeachBoard.MembersService.Application.Validation;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.WebApi.ActionResults;
using TeachBoard.MembersService.WebApi.Models.Teacher;

namespace TeachBoard.MembersService.WebApi.Controllers;

[ValidateModel]
[ApiController]
[Route("teachers")]
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
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Teacher), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Teacher>> GetById(int id)
    {
        var query = new GetTeacherByIdQuery { TeacherId = id };
        var teacher = await _mediator.Send(query);

        return new WebApiResult(teacher);
    }

    /// <summary>
    /// Create teacher
    /// </summary>
    /// <param name="model">Teacher creation model</param>
    /// <response code="200">Success. Teacher created</response>
    /// <response code="409">Teacher with given user id already exists (teacher_already_exists)</response>
    [HttpPost]
    [ProducesResponseType(typeof(Teacher), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Teacher>> Create([FromBody] CreateTeacherRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        var command = new CreateTeacherCommand { UserId = model.UserId };
        var teacher = await _mediator.Send(command);

        return new WebApiResult(teacher);
    }

    /// <summary>
    /// Get all teachers
    /// </summary>
    /// <response code="200">Success. Teachers returns.</response>
    /// <response code="404">Teachers not found (teachers_not_found)</response>
    [HttpGet]
    [ProducesResponseType(typeof(IList<Teacher>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IList<Teacher>>> GetAll()
    {
        var query = new GetAllTeachersQuery();
        var teacher = await _mediator.Send(query);

        return new WebApiResult(teacher);
    }

    /// <summary>
    /// Delete teacher by user id
    /// </summary>
    /// <response code="200">Success. Teacher deleted</response>
    /// <response code="404">Teacher not found (teacher_not_found)</response>
    [HttpDelete("by-user/{userId:int}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteByUserId(int userId)
    {
        // Delete by id
        await _mediator.Send(new DeleteTeacherByUserIdCommand { UserId = userId });

        return new WebApiResult();
    }

    /// <summary>
    /// Get teachers by ids
    /// </summary>
    /// <param name="teacherId">List of teachers ids</param>
    /// <response code="200">Success. Teachers returned</response>
    /// <response code="404">Teachers with given id not found (teachers_not_found)</response>
    [HttpGet("by-ids")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IList<Teacher>>> GetTeachersByIds([FromQuery] List<int> teacherId)
    {
        var query = new GetTeachersByIdsQuery { Ids = teacherId };
        var teachers = await _mediator.Send(query);

        return new WebApiResult(teachers);
    }
}
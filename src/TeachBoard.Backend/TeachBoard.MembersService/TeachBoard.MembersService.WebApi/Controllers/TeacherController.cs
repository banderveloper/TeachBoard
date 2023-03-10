using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.MembersService.Application.Features.Teachers;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.WebApi.ActionResults;
using TeachBoard.MembersService.WebApi.Models.Teacher;
using TeachBoard.MembersService.WebApi.Validation;

namespace TeachBoard.MembersService.WebApi.Controllers;

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
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Teacher), StatusCodes.Status200OK)]
    public async Task<ActionResult<Teacher>> GetById(int id)
    {
        var query = new GetTeacherByIdQuery { TeacherId = id };
        var teacher = await _mediator.Send(query);

        return new WebApiResult(teacher);
    }

    /// <summary>
    /// Create teacher
    /// </summary>
    /// <param name="model">New teacher data</param>
    /// <response code="200">Success / teacher_already_exists</response>
    /// <response code="422">Invalid model</response>
    [HttpPost]
    [ProducesResponseType(typeof(Teacher), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<Teacher>> Create([FromBody] CreateTeacherRequestModel model)
    {
        var command = new CreateTeacherCommand { UserId = model.UserId };
        var teacher = await _mediator.Send(command);

        return new WebApiResult(teacher);
    }

    /// <summary>
    /// Get all teachers
    /// </summary>
    /// <response code="200">Success</response>
    [HttpGet]
    [ProducesResponseType(typeof(IList<Teacher>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<Teacher>>> GetAll()
    {
        var query = new GetAllTeachersQuery();
        var teacher = await _mediator.Send(query);

        return new WebApiResult(teacher);
    }

    /// <summary>
    /// Delete teacher by user id
    /// </summary>
    /// <response code="200">Success / teacher_not_found</response>
    [HttpDelete("by-user/{userId:int}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteByUserId(int userId)
    {
        var command = new DeleteTeacherByUserIdCommand { UserId = userId };
        // Delete by id
        await _mediator.Send(command);

        return new WebApiResult();
    }

    /// <summary>
    /// Get teachers by ids
    /// </summary>
    /// <param name="teacherId">List of teachers ids</param>
    /// <response code="200">Success. Teachers returned</response>
    [HttpGet("by-ids")]
    [ProducesResponseType(typeof(IList<Teacher>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<Teacher>>> GetTeachersByIds([FromQuery] List<int> teacherId)
    {
        var query = new GetTeachersByIdsQuery { Ids = teacherId };
        var teachers = await _mediator.Send(query);

        return new WebApiResult(teachers);
    }

    /// <summary>
    /// Get teacher by user id
    /// </summary>
    /// <param name="userId">Teacher's user id</param>
    /// <response code="200">Success. Teacher or null returned </response>
    [HttpGet("by-user-id/{userId:int}")]
    [ProducesResponseType(typeof(Teacher), StatusCodes.Status200OK)]
    public async Task<ActionResult<Teacher?>> GetTeacherByUserId(int userId)
    {
        var query = new GetTeacherByUserIdQuery { UserId = userId };
        var teacher = await _mediator.Send(query);

        return new WebApiResult(teacher);
    }
}
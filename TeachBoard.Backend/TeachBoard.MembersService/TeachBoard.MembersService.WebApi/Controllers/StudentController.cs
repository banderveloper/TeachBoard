using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.MembersService.Application.Features.Students;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.WebApi.ActionResults;
using TeachBoard.MembersService.WebApi.Models.Student;
using TeachBoard.MembersService.WebApi.Validation;

namespace TeachBoard.MembersService.WebApi.Controllers;

[ApiController]
[Route("students")]
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

    /// <summary>
    /// Get student by id
    /// </summary>
    /// <param name="id">Student id</param>
    /// <response code="200">Success</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    public async Task<ActionResult<Student>> GetStudentById(int id)
    {
        var query = new GetStudentByIdQuery { StudentId = id };
        var student = await _mediator.Send(query);

        return new WebApiResult(student);
    }

    /// <summary>
    /// Get students by group id
    /// </summary>
    /// <param name="groupId">Group id</param>
    /// <response code="200">Success</response>
    [HttpGet("by-group/{groupId:int}")]
    [ProducesResponseType(typeof(IList<Student>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<Student>>> GetStudentsByGroupId(int groupId)
    {
        var query = new GetStudentsByGroupIdQuery { GroupId = groupId };
        var students = await _mediator.Send(query);

        return new WebApiResult(students);
    }

    /// <summary>
    /// Create student
    /// </summary>
    /// <param name="model">New student data</param>
    /// <response code="200">Success / group_not_found / student_already_exists</response>
    /// <response code="422">Invalid model</response>
    [HttpPost]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<Student>> CreateStudent([FromBody] CreateStudentRequestModel model)
    {
        var command = new CreateStudentCommand { GroupId = model.GroupId, UserId = model.UserId };
        var student = await _mediator.Send(command);

        return new WebApiResult(student);
    }

    /// <summary>
    /// Delete student by user id
    /// </summary>
    /// <response code="200">Success / student_not_found</response>
    [HttpDelete("by-user/{userId:int}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteByUserId(int userId)
    {
        // Delete by id
        await _mediator.Send(new DeleteStudentByUserIdCommand { UserId = userId });

        return new WebApiResult();
    }

    /// <summary>
    /// Get students from group of given student by student id
    /// </summary>
    /// <param name="studentId">Student id</param>
    /// <response code="200">Success</response>
    [HttpGet("group-members-by-student/{studentId:int}")]
    [ProducesResponseType(typeof(IList<Student>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<Student>>> GetStudentGroupMembersByStudentId(int studentId)
    {
        var query = new GetStudentGroupMembersByStudentIdQuery { StudentId = studentId };
        var students = await _mediator.Send(query);

        return new WebApiResult(students);
    }

    /// <summary>
    /// Get students from group of given student by user id
    /// </summary>
    /// <param name="userId">User id</param>
    /// <response code="200">Success. Array of students returns</response>
    [HttpGet("group-members-by-user/{userId:int}")]
    [ProducesResponseType(typeof(IList<Student>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<Student>>> GetStudentGroupMembersByUserId(int userId)
    {
        var query = new GetStudentGroupMembersByUserIdQuery { UserId = userId };
        var students = await _mediator.Send(query);

        return new WebApiResult(students);
    }

    /// <summary>
    /// Get student by user id
    /// </summary>
    /// <param name="userId">User id</param>
    /// <response code="200">Success</response>
    [HttpGet("by-user/{userId:int}")]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    public async Task<ActionResult<Student>> GetStudentByUserId(int userId)
    {
        var query = new GetStudentByUserIdQuery { UserId = userId };
        var student = await _mediator.Send(query);

        return new WebApiResult(student);
    }

    /// <summary>
    /// Set student group
    /// </summary>
    /// <param name="model">Student id and group id</param>
    /// <response code="200">Success</response>
    /// <response code="422">Invalid model</response>
    [HttpPut("student-group")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> SetStudentGroup([FromBody] SetStudentGroupRequestModel model)
    {
        var command = _mapper.Map<SetStudentGroupCommand>(model);
        await _mediator.Send(command);

        return new WebApiResult();
    }
}
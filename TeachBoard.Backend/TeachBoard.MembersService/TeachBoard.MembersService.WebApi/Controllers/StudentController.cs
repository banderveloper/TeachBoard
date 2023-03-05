using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Features.Students;
using TeachBoard.MembersService.Application.Features.Students.Common;
using TeachBoard.MembersService.Application.Validation;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.WebApi.ActionResults;
using TeachBoard.MembersService.WebApi.Models.Student;

namespace TeachBoard.MembersService.WebApi.Controllers;

[ValidateModel]
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
    /// <response code="200">Success. Student returns.</response>
    /// <response code="404">Student with given id not found (student_not_found)</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
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
    /// <response code="200">Success. Array of students returns</response>
    /// <response code="404">Students with given group id not found (students_not_found)</response>
    [HttpGet("by-group/{groupId:int}")]
    [ProducesResponseType(typeof(StudentsListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentsListModel>> GetStudentsByGroupId(int groupId)
    {
        var query = new GetStudentsByGroupIdQuery { GroupId = groupId };
        var students = await _mediator.Send(query);

        return new WebApiResult(students);
    }

    /// <summary>
    /// Create student
    /// </summary>
    /// <param name="model">Student creation model</param>
    /// <response code="200">Success. Student created</response>
    /// <response code="404">Group with given group id not found (group_not_found)</response>
    /// <response code="409">Student with given user id already exists (student_already_exists)</response>
    [HttpPost]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Student>> CreateStudent([FromBody] CreateStudentRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        var command = new CreateStudentCommand { GroupId = model.GroupId, UserId = model.UserId };
        var student = await _mediator.Send(command);

        return new WebApiResult(student);
    }

    /// <summary>
    /// Delete student by user id
    /// </summary>
    /// <response code="200">Success. Student deleted</response>
    /// <response code="404">Student not found (student_not_found)</response>
    [HttpDelete("by-user/{userId:int}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
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
    /// <response code="200">Success. Array of students returns</response>
    /// <response code="404">Student with given id not found (student_not_found) / Student does not belong to any group (group_not_found)</response>
    [HttpGet("group-members-by-student/{studentId:int}")]
    [ProducesResponseType(typeof(StudentsListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentsListModel>> GetStudentGroupMembersByStudentId(int studentId)
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
    /// <response code="404">Student with given user id not found (student_not_found) / Student does not belong to any group (group_not_found)</response>
    [HttpGet("group-members-by-user/{userId:int}")]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentsListModel>> GetStudentGroupMembersByUserId(int userId)
    {
        var query = new GetStudentGroupMembersByUserIdQuery { UserId = userId };
        var students = await _mediator.Send(query);

        return new WebApiResult(students);
    }

    /// <summary>
    /// Get student by user id
    /// </summary>
    /// <param name="userId">User id</param>
    /// <response code="200">Success. Student returned</response>
    /// <response code="404">Student with given user id not found (student_not_found</response>
    [HttpGet("by-user/{userId:int}")]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Student>> GetStudentByUserId(int userId)
    {
        var query = new GetStudentByUserIdQuery { UserId = userId };
        var student = await _mediator.Send(query);

        return new WebApiResult(student);
    }

    /// <summary>
    /// Set student group
    /// </summary>
    /// <param name="model">Model with student id and group id</param>
    /// <response code="200">Success. Student added to group</response>
    /// <response code="404">
    /// Student with given id not found (student_not_found)
    /// Group with given id not found (group_not_found)
    /// </response>
    [HttpPut("student-group")]
    public async Task<IActionResult> SetStudentGroup([FromBody] SetStudentGroupRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        var command = _mapper.Map<SetStudentGroupCommand>(model);
        await _mediator.Send(command);

        return new WebApiResult();
    }
}
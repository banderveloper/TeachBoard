using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Features.Students;
using TeachBoard.MembersService.Application.Features.Students.Common;
using TeachBoard.MembersService.Application.Validation;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.WebApi.Models.Student;

namespace TeachBoard.MembersService.WebApi.Controllers;

[ValidateModel]
[ApiController]
[Route("members/student")]
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
    [HttpGet("getbyid/{id:int}")]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Student>> GetStudentById(int id)
    {
        var query = new GetStudentByIdQuery { StudentId = id };
        var student = await _mediator.Send(query);

        return Ok(student);
    }

    /// <summary>
    /// Get students by group id
    /// </summary>
    /// <param name="id">Group id</param>
    /// <response code="200">Success. Array of students returns</response>
    /// <response code="404">Students with given group id not found (students_not_found)</response>
    [HttpGet("getbygroupid/{id:int}")]
    [ProducesResponseType(typeof(StudentsListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentsListModel>> GetStudentsByGroupId(int id)
    {
        var query = new GetStudentsByGroupIdQuery { GroupId = id };
        var students = await _mediator.Send(query);

        return Ok(students);
    }

    /// <summary>
    /// Create student
    /// </summary>
    /// <param name="model">Student creation model</param>
    /// <response code="200">Success. Student created</response>
    /// <response code="404">Group with given group id not found (group_not_found)</response>
    /// <response code="409">Student with given user id already exists (student_already_exists)</response>
    [HttpPost("create")]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Student>> CreateStudent([FromBody] CreateStudentRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        var command = new CreateStudentCommand { GroupId = model.GroupId, UserId = model.UserId };
        var student = await _mediator.Send(command);

        return student;
    }

    /// <summary>
    /// Delete student by user id
    /// </summary>
    /// <response code="200">Success. Student deleted</response>
    /// <response code="404">Student not found (student_not_found)</response>
    [HttpDelete("deletebyuserid/{id:int}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteByUserId(int id)
    {
        // Delete by id
        await _mediator.Send(new DeleteStudentByUserIdCommand { UserId = id });

        return Ok();
    }

    /// <summary>
    /// Get students from group of given student
    /// </summary>
    /// <param name="studentId">Group id</param>
    /// <response code="200">Success. Array of students returns</response>
    /// <response code="404">Student with given id not found (student_not_found)</response>
    [HttpGet("getgroupmembers")]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentsListModel>> GetStudentGroupMembers(int studentId)
    {
        var query = new GetStudentGroupMembersQuery { StudentId = studentId };
        var students = await _mediator.Send(query);

        return Ok(students);
    }
}
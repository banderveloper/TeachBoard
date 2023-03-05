using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Features.Feedbacks;
using TeachBoard.MembersService.Application.Validation;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.Domain.Enums;
using TeachBoard.MembersService.WebApi.ActionResults;
using TeachBoard.MembersService.WebApi.Models.Feedback;

namespace TeachBoard.MembersService.WebApi.Controllers;

[ValidateModel]
[ApiController]
[Route("feedbacks")]
[Produces("application/json")]
public class FeedbackController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public FeedbackController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all teacher to student feedbacks
    /// </summary>
    /// <response code="200">Success. Feedbacks returns</response>
    /// <response code="404">Teacher to student feedbacks not found (feedbacks_not_found)</response>
    [HttpGet("teacher-student")]
    [ProducesResponseType(typeof(IList<Feedback>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IList<Feedback>>> GetAllTeacherToStudentFeedbacks()
    {
        var query = new GetAllFeedbacksByDirectionQuery { Direction = FeedbackDirection.TeacherToStudent };
        var feedbacks = await _mediator.Send(query);

        return new WebApiResult(feedbacks);
    }

    /// <summary>
    /// Get all student to teacher feedbacks
    /// </summary>
    /// <response code="200">Success. Feedbacks returns</response>
    /// <response code="404">Student to teacher feedbacks not found (feedbacks_not_found)</response>
    [HttpGet("student-teacher")]
    [ProducesResponseType(typeof(IList<Feedback>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IList<Feedback>>> GetAllStudentToTeacherFeedbacks()
    {
        var query = new GetAllFeedbacksByDirectionQuery { Direction = FeedbackDirection.StudentToTeacher };
        var feedbacks = await _mediator.Send(query);

        return new WebApiResult(feedbacks);
    }

    /// <summary>
    /// Create teacher to student feedback
    /// </summary>
    /// <response code="200">Success. Feedback created and returned.</response>
    /// <response code="404">Student/teacher by student/teacher id not found (student_not_found) (teacher_not_found)</response>
    [HttpPost("teacher-student")]
    [ProducesResponseType(typeof(Feedback), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Feedback>> CreateTeacherToStudentFeedback(
        [FromBody] CreateFeedbackRequestModel model)
    {
        var command = _mapper.Map<CreateFeedbackCommand>(model);
        command.Direction = FeedbackDirection.TeacherToStudent;

        var feedback = await _mediator.Send(command);

        return new WebApiResult(feedback);
    }

    /// <summary>
    /// Create student to teacher feedback
    /// </summary>
    /// <response code="200">Success. Feedback created and returned.</response>
    /// <response code="404">Student/teacher by student/teacher id not found (student_not_found) (teacher_not_found)</response>
    [HttpPost("student-teacher")]
    [ProducesResponseType(typeof(Feedback), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Feedback>> CreateStudentToTeacherFeedback(
        [FromBody] CreateFeedbackRequestModel model)
    {
        var command = _mapper.Map<CreateFeedbackCommand>(model);
        command.Direction = FeedbackDirection.StudentToTeacher;

        var feedback = await _mediator.Send(command);

        return new WebApiResult(feedback);
    }

    /// <summary>
    /// Get feedbacks by student id
    /// </summary>
    /// <response code="200">Success. Feedbacks returns</response>
    /// <response code="404">Feedbacks with given student id not found (feedbacks_not_found)</response>
    [HttpGet("by-student/{studentId:int}")]
    [ProducesResponseType(typeof(IList<Feedback>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IList<Feedback>>> GetFeedbacksByStudentId(int studentId)
    {
        var query = new GetFeedbacksByStudentIdQuery { StudentId = studentId };
        var feedbacks = await _mediator.Send(query);

        return new WebApiResult(feedbacks);
    }

    /// <summary>
    /// Get feedbacks by teacher id
    /// </summary>
    /// <response code="200">Success. Feedbacks returns</response>
    /// <response code="404">Feedbacks with given teacher id not found (feedbacks_not_found)</response>
    [HttpGet("by-teacher/{teacherId:int}")]
    [ProducesResponseType(typeof(IList<Feedback>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IList<Feedback>>> GetFeedbacksByTeacherId(int teacherId)
    {
        var query = new GetFeedbacksByTeacherIdQuery { TeacherId = teacherId };
        var feedbacks = await _mediator.Send(query);

        return new WebApiResult(feedbacks);
    }
}
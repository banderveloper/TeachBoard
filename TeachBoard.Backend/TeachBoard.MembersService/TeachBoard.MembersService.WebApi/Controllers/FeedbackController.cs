using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.MembersService.Application.Features.Feedbacks;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.Domain.Enums;
using TeachBoard.MembersService.WebApi.ActionResults;
using TeachBoard.MembersService.WebApi.Models.Feedback;
using TeachBoard.MembersService.WebApi.Validation;

namespace TeachBoard.MembersService.WebApi.Controllers;

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
    /// <response code="200">Success</response>
    [HttpGet("teacher-student")]
    [ProducesResponseType(typeof(IList<Feedback>), StatusCodes.Status200OK)]
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
    [HttpGet("student-teacher")]
    [ProducesResponseType(typeof(IList<Feedback>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<Feedback>>> GetAllStudentToTeacherFeedbacks()
    {
        var query = new GetAllFeedbacksByDirectionQuery { Direction = FeedbackDirection.StudentToTeacher };
        var feedbacks = await _mediator.Send(query);

        return new WebApiResult(feedbacks);
    }

    /// <summary>
    /// Create teacher to student feedback
    /// </summary>
    /// <param name="model">Feedback data</param>
    /// <response code="200">Success / student_not_found / teacher_not_found</response>
    /// <response code="422">Invalid model</response>
    [HttpPost("teacher-student")]
    [ProducesResponseType(typeof(Feedback), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
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
    /// <param name="model">Feedback data</param>
    /// <response code="200">Success / student_not_found / teacher_not_found</response>
    /// <response code="422">Invalid model</response>
    [HttpPost("student-teacher")]
    [ProducesResponseType(typeof(Feedback), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
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
    /// <param name="studentId">Student id</param>
    /// <response code="200">Success</response>
    [HttpGet("by-student/{studentId:int}")]
    [ProducesResponseType(typeof(IList<Feedback>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<Feedback>>> GetFeedbacksByStudentId(int studentId)
    {
        var query = new GetFeedbacksByStudentIdQuery { StudentId = studentId };
        var feedbacks = await _mediator.Send(query);

        return new WebApiResult(feedbacks);
    }

    /// <summary>
    /// Get feedbacks by teacher id
    /// </summary>
    /// <param name="teacherId">Teacher id</param>
    /// <response code="200">Success</response>
    [HttpGet("by-teacher/{teacherId:int}")]
    [ProducesResponseType(typeof(IList<Feedback>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<Feedback>>> GetFeedbacksByTeacherId(int teacherId)
    {
        var query = new GetFeedbacksByTeacherIdQuery { TeacherId = teacherId };
        var feedbacks = await _mediator.Send(query);

        return new WebApiResult(feedbacks);
    }
}
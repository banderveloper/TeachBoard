using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Features.Feedbacks;
using TeachBoard.MembersService.Application.Validation;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.WebApi.Models.Feedback;

namespace TeachBoard.MembersService.WebApi.Controllers;

[ValidateModel]
[ApiController]
[Route("members/feedback")]
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
    [HttpGet("getallteacherstudent")]
    [ProducesResponseType(typeof(TeacherToStudentFeedbacksListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeacherToStudentFeedbacksListModel>> GetAllTeacherToStudentFeedbacks()
    {
        var query = new GetAllTeacherToStudentFeedbacksQuery();
        var feedbacksModel = await _mediator.Send(query);

        return Ok(feedbacksModel);
    }

    /// <summary>
    /// Get all student to teacher feedbacks
    /// </summary>
    /// <response code="200">Success. Feedbacks returns</response>
    /// <response code="404">Teacher to student feedbacks not found (feedbacks_not_found)</response>
    [HttpGet("getallstudentteacher")]
    [ProducesResponseType(typeof(StudentToTeacherFeedbacksListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentToTeacherFeedbacksListModel>> GetAllStudentToTeacherFeedbacks()
    {
        var query = new GetAllStudentToTeacherFeedbacksQuery();
        var feedbacksModel = await _mediator.Send(query);

        return Ok(feedbacksModel);
    }

    /// <summary>
    /// Create teacher to student feedback
    /// </summary>
    /// <response code="200">Success. Feedback created and returned.</response>
    /// <response code="404">Student/teacher by student/teacher id not found (student_not_found) (teacher_not_found)</response>
    [HttpPost("createteacherstudent")]
    [ProducesResponseType(typeof(TeacherToStudentFeedback), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeacherToStudentFeedback>> CreateTeacherToStudentFeedback(
        [FromBody] CreateTeacherToStudentRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);
        
        var command = _mapper.Map<CreateTeacherToStudentFeedbackCommand>(model);
        var feedback = await _mediator.Send(command);

        return Ok(feedback);
    }

    /// <summary>
    /// Create student to teacher feedback
    /// </summary>
    /// <response code="200">Success. Feedback created and returned.</response>
    /// <response code="404">Student/teacher by student/teacher id not found (student_not_found) (teacher_not_found)</response>
    [HttpPost("createstudentteacher")]
    [ProducesResponseType(typeof(StudentToTeacherFeedback), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentToTeacherFeedback>> CreateStudentToTeacherFeedback(
        [FromBody] CreateStudentToTeacherRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);
        
        var command = _mapper.Map<CreateStudentToTeacherFeedbackCommand>(model);
        var feedback = await _mediator.Send(command);

        return Ok(feedback);
    }
}
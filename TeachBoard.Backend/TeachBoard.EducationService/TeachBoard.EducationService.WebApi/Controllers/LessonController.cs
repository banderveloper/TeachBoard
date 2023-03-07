using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Features.Lesson;
using TeachBoard.EducationService.Application.Features.StudentLessonActivity;
using TeachBoard.EducationService.Domain.Entities;
using TeachBoard.EducationService.WebApi.ActionResults;
using TeachBoard.EducationService.WebApi.Models.Lesson;
using TeachBoard.EducationService.WebApi.Validation;

namespace TeachBoard.EducationService.WebApi.Controllers;

[ApiController]
[Route("lessons")]
[Produces("application/json")]
public class LessonController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public LessonController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Create new lesson
    /// </summary>
    /// 
    /// <param name="model">Create lesson model</param>
    /// <returns>Created lesson</returns>
    ///
    /// <response code="200">Success / subject_not_found</response>
    /// <response code="400">invalid_date_time</response>
    /// <response code="422">Invalid model</response>
    [HttpPost]
    [ProducesResponseType(typeof(Lesson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IBadRequestApiException), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<Lesson>> CreateLesson([FromBody] CreateLessonRequestModel model)
    {
        var command = _mapper.Map<CreateLessonCommand>(model);
        var createdLesson = await _mediator.Send(command);

        return new WebApiResult(createdLesson);
    }

    /// <summary>
    /// Get future lessons
    /// </summary>
    /// 
    /// <returns>Future lessons</returns>
    ///
    /// <response code="200">Success. Lesson returned</response>
    [HttpGet("future")]
    [ProducesResponseType(typeof(IList<Lesson>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<Lesson>>> GetFutureLessons()
    {
        var query = new GetFutureLessonsQuery();
        var lessons = await _mediator.Send(query);

        return new WebApiResult(lessons);
    }

    /// <summary>
    /// Get lessons by group id
    /// </summary>
    /// 
    /// <returns>Group lessons</returns>
    ///
    /// <response code="200">Success. Lessons returned</response>
    [HttpGet("by-group/{groupId:int}")]
    [ProducesResponseType(typeof(IList<LessonPresentationDataModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<LessonPresentationDataModel>>> GetLessonsByGroupId(int groupId)
    {
        var query = new GetLessonsPresentationDataByGroupIdQuery { GroupId = groupId };
        var lessons = await _mediator.Send(query);

        return new WebApiResult(lessons);
    }

    /// <summary>
    /// Create/update student lesson activity
    /// </summary>
    ///
    /// <param name="model">Set student lesson activity model</param>
    /// <returns>Created or updated student lesson activity</returns>
    ///
    /// <response code="200">Success / lesson_not_found / lesson_not_started</response>
    /// <response code="422">Invalid model</response>
    [HttpPost("student-activity")]
    [ProducesResponseType(typeof(StudentLessonActivity), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<StudentLessonActivity>> SetStudentLessonActivity(
        [FromBody] SetStudentLessonActivityModel model)
    {
        var command = _mapper.Map<SetStudentLessonActivityCommand>(model);

        var studentActivity = await _mediator.Send(command);

        return new WebApiResult(studentActivity);
    }

    /// <summary>
    /// Get student lesson activities by student id
    /// </summary>
    ///
    /// <param name="studentId">Student id</param>
    /// <returns>Student lesson activities</returns>
    ///
    /// <response code="200">Success. Student lesson activities returned</response>
    [HttpGet("student-activities/{studentId:int}")]
    [ProducesResponseType(typeof(IList<StudentLessonActivityPresentationDataModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<StudentLessonActivityPresentationDataModel>>> GetStudentLessonActivitiesByStudentId(
        int studentId)
    {
        var query = new GetStudentLessonActivitiesPresentationDataByStudentIdQuery { StudentId = studentId };

        var activities = await _mediator.Send(query);
        return new WebApiResult(activities);
    }
}
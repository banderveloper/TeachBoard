using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Features.Lesson;
using TeachBoard.EducationService.Application.Features.StudentLessonActivity;
using TeachBoard.EducationService.Domain.Entities;
using TeachBoard.EducationService.WebApi.Models.Lesson;
using TeachBoard.EducationService.WebApi.Models.Validation;

namespace TeachBoard.EducationService.WebApi.Controllers;

[ApiController]
[Route("lessons")]
[ValidateModel]
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
    /// <response code="200">Success. Lesson created and returned</response>
    /// <response code="404">Subject with given id not found (subject_not_found)</response>
    /// <response code="422">Invalid model</response>
    [HttpPost("create")]
    [ProducesResponseType(typeof(Lesson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<Lesson>> CreateLesson([FromBody] CreateLessonRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        var command = _mapper.Map<CreateLessonCommand>(model);
        var createdLesson = await _mediator.Send(command);

        return Ok(createdLesson);
    }

    /// <summary>
    /// Get future lessons
    /// </summary>
    /// 
    /// <returns>Future lessons</returns>
    ///
    /// <response code="200">Success. Lesson returned</response>
    /// <response code="404">Future lessons not found (lessons_not_found)</response>
    [HttpGet("getFuture")]
    [ProducesResponseType(typeof(LessonsListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LessonsListModel>> GetFutureLessons()
    {
        var query = new GetFutureLessonsQuery();
        var lessonsModel = await _mediator.Send(query);

        return Ok(lessonsModel);
    }

    /// <summary>
    /// Get lessons by group id
    /// </summary>
    /// 
    /// <returns>Group lessons</returns>
    ///
    /// <response code="200">Success. Lessons returned</response>
    /// <response code="404">Lessons not found (lessons_not_found)</response>
    [HttpGet("getByGroupId/{id:int}")]
    [ProducesResponseType(typeof(LessonsListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LessonsListModel>> GetLessonsByGroupId(int id)
    {
        var query = new GetLessonsByGroupIdQuery { GroupId = id };
        var lessonsModel = await _mediator.Send(query);

        return Ok(lessonsModel);
    }

    /// <summary>
    /// Create/update student lesson activity
    /// </summary>
    ///
    /// <param name="model">Set student lesson activity model</param>
    /// <returns>Created or updated student lesson activity</returns>
    ///
    /// <response code="200">Success. Activity created / updated</response>
    /// <response code="400">Setting lesson activity to future lessons is not allowed (lesson_not_started)</response>
    /// <response code="404">Lesson with given id not found (lesson_not_found)</response>
    /// <response code="422">Invalid model</response>
    [HttpPost("setStudentActivity")]
    [ProducesResponseType(typeof(StudentLessonActivity), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<StudentLessonActivity>> SetStudentLessonActivity(
        [FromBody] SetStudentLessonActivityModel model)
    {
        var command = _mapper.Map<SetStudentLessonActivityCommand>(model);

        var studentActivity = await _mediator.Send(command);

        return studentActivity;
    }

    /// <summary>
    /// Get student lesson activites by student id
    /// </summary>
    ///
    /// <param name="studentId">Student id</param>
    /// <returns>Student lesson activities</returns>
    ///
    /// <response code="200">Success. Student lesson activities returned</response>
    /// <response code="404">Student lesson activities witn given student id not found (student_lesson_activities_not_found)</response>
    [HttpGet("getStudentActivities/{studentId:int}")]
    [ProducesResponseType(typeof(StudentLessonActivityPublicListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentLessonActivityPublicListModel>> GetStudentLessonActivitiesByStudentId(
        int studentId)
    {
        var query = new GetStudentLessonActivitiesByStudentIdQuery { StudentId = studentId };

        var activitiesListModel = await _mediator.Send(query);
        return Ok(activitiesListModel);
    }
}
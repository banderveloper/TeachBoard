using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Features.Homework;
using TeachBoard.EducationService.Domain.Entities;
using TeachBoard.EducationService.WebApi.ActionResults;
using TeachBoard.EducationService.WebApi.Models.Homework;
using TeachBoard.EducationService.WebApi.Validation;

namespace TeachBoard.EducationService.WebApi.Controllers;

[Route("homeworks")]
[ApiController]
[Produces("application/json")]
public class HomeworkController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public HomeworkController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Create new homework
    /// </summary>
    /// 
    /// <param name="model">Create homework requestModel</param>
    /// <returns>Created homework</returns>
    ///
    /// <response code="200">Success </response>
    /// <response code="422">Invalid model</response>
    [HttpPost]
    [ProducesResponseType(typeof(Homework), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<Homework>> CreateHomework([FromBody] CreateHomeworkRequestModel model)
    {
        var command = _mapper.Map<CreateHomeworkCommand>(model);
        var createdHomework = await _mediator.Send(command);

        return new WebApiResult(createdHomework);
    }

    /// <summary>
    /// Get homeworks for group with given id
    /// </summary>
    /// 
    /// <param name="groupId">Id of the group whose homework to return</param>
    /// <returns>List of homeworks for group</returns>
    ///
    /// <response code="200">Success</response>>
    [HttpGet("group/{groupId:int}")]
    [ProducesResponseType(typeof(IList<Homework>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<Homework>>> GetHomeworksByGroupId(int groupId)
    {
        var query = new GetHomeworksByGroupIdQuery { GroupId = groupId };
        var homeworks = await _mediator.Send(query);

        return new WebApiResult(homeworks);
    }

    /// <summary>
    /// Complete given by teacher homework as student
    /// </summary>
    /// 
    /// <param name="model">Completing homework data</param>
    /// <returns>Completed homework</returns>
    ///
    /// <response code="200">Success / homework_not_found / completed_homework_already_exists</response>
    /// <response code="422">Invalid model</response>
    [HttpPost("complete")]
    [ProducesResponseType(typeof(CompletedHomework), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<CompletedHomework>> CompleteHomework([FromBody] CompleteHomeworkRequestModel model)
    {
        var command = _mapper.Map<CompleteHomeworkCommand>(model);
        var completedHomework = await _mediator.Send(command);

        return new WebApiResult(completedHomework);
    }

    /// <summary>
    /// Check completed homework (set grade and comment)
    /// </summary>
    /// 
    /// <param name="model">Checking homework data</param>
    /// <returns>Checked completed homework</returns>
    ///
    /// <response code="200">Success / completed_homework_not_found / completed_homework_invalid_teacher</response>
    /// <response code="422">Invalid model</response>
    [HttpPost("check-completed")]
    [ProducesResponseType(typeof(CompletedHomework), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<CompletedHomework>> CheckHomework([FromBody] CheckHomeworkRequestModel model)
    {
        var command = _mapper.Map<CheckHomeworkCommand>(model);
        var completedHomework = await _mediator.Send(command);

        return new WebApiResult(completedHomework);
    }

    /// <summary>
    /// Get full information about student's completed homeworks
    /// </summary>
    /// 
    /// <param name="studentId">Student id</param>
    /// <returns>List of completed homeworks full data</returns>
    ///
    /// <response code="200">Success. Completed homeworks full data by student returned</response>
    [HttpGet("completed/{studentId:int}")]
    [ProducesResponseType(typeof(IList<CompletedHomeworkPresentationDataModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<CompletedHomeworkPresentationDataModel>>>
        GetFullCompletedHomeworksByStudentId(int studentId)
    {
        var query = new GetCompletedHomeworksPresentationDataByStudentIdQuery { StudentId = studentId };
        var completedHomeworks = await _mediator.Send(query);

        return new WebApiResult(completedHomeworks);
    }

    /// <summary>
    /// Get student's uncompleted homeworks
    /// </summary>
    /// 
    /// <param name="studentId">Student id</param>
    /// <param name="groupId">Student's group id</param>
    /// <returns>List of uncompleted homeworks presentation</returns>
    ///
    /// <response code="200">Success. Uncompleted homeworks full data by student returned</response>
    [HttpGet("uncompleted-homeworks/{studentId:int}/{groupId:int}")]
    [ProducesResponseType(typeof(IList<UncompletedHomeworkPresentationDataModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<UncompletedHomeworkPresentationDataModel>>> GetUncompletedHomeworksByStudent(
        int studentId,
        int groupId)
    {
        var query = new GetUncompletedHomeworksPresentationDataByStudentQuery
            { StudentId = studentId, GroupId = groupId };
        var uncompletedHomeworks = await _mediator.Send(query);

        return new WebApiResult(uncompletedHomeworks);
    }

    /// <summary>
    /// Get list of teacher's unchecked homeworks count
    /// </summary>
    /// 
    /// <returns>List of teacher's unchecked homeworks count</returns>
    ///
    /// <response code="200">Success</response>
    [HttpGet("teachers-unchecked-count")]
    [ProducesResponseType(typeof(IList<TeacherUncheckedHomeworksCountModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<TeacherUncheckedHomeworksCountModel>>> GetTeachersUncheckedHomeworksCount()
    {
        var query = new GetTeachersUncheckedHomeworksCountQuery();

        var teachersUncheckedCount = await _mediator.Send(query);

        return new WebApiResult(teachersUncheckedCount);
    }

    /// <summary>
    /// Get teacher's unchecked homeworks
    /// </summary>
    /// 
    /// <returns>List of teacher's unchecked homeworks</returns>
    ///
    /// <response code="200">Success</response>
    [HttpGet("teacher-unchecked-homeworks/{teacherId:int}")]
    [ProducesResponseType(typeof(IList<CompletedHomework>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<CompletedHomework>>> GetTeacherUncheckedHomeworks(int teacherId)
    {
        var query = new GetTeacherUncheckedHomeworksQuery { TeacherId = teacherId };
        var uncheckedHomeworks = await _mediator.Send(query);

        return new WebApiResult(uncheckedHomeworks);
    }
}
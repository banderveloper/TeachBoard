using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Features.Homework;
using TeachBoard.EducationService.Domain.Entities;
using TeachBoard.EducationService.WebApi.Models.Homework;
using TeachBoard.EducationService.WebApi.Models.Validation;

namespace TeachBoard.EducationService.WebApi.Controllers;

[Route("homeworks")]
[ApiController]
[Produces("application/json")]
[ValidateModel]
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
    /// <param name="requestModel">Create homework requestModel</param>
    /// <returns>Created homework</returns>
    ///
    /// <response code="200">Success. Homework created and returned</response>
    /// <response code="422">Invalid requestModel</response>
    [HttpPost]
    [ProducesResponseType(typeof(Homework), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<Homework>> CreateHomework([FromBody] CreateHomeworkRequestModel requestModel)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(requestModel);

        var command = _mapper.Map<CreateHomeworkCommand>(requestModel);
        var createdHomework = await _mediator.Send(command);

        return createdHomework;
    }

    /// <summary>
    /// Get homeworks for group with given id
    /// </summary>
    /// 
    /// <param name="groupId">Id of the group whose homework to return</param>
    /// <returns>Created homewirk</returns>
    ///
    /// <response code="200">Success. Homeworks for given group returned</response>
    /// <response code="404">Homeworks for given group not found (homeworks_not_found)</response>
    [HttpGet("group/{groupId:int}")]
    [ProducesResponseType(typeof(HomeworksListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HomeworksListModel>> GetHomeworksByGroupId(int groupId)
    {
        var query = new GetHomeworksByGroupIdQuery { GroupId = groupId };
        var homeworksModel = await _mediator.Send(query);

        return homeworksModel;
    }

    /// <summary>
    /// Сomplete given by teacher homework as student
    /// </summary>
    /// 
    /// <param name="model">Completing homework model</param>
    /// <returns>Completed homework</returns>
    ///
    /// <response code="200">Success. Homework completed and returned</response>
    /// <response code="404">Homework with given id not found (homework_not_found)</response>
    /// <response code="409">Student already completed this homework (completed_homework_already_exists)</response>
    /// <response code="422">Invalid requestModel</response>
    [HttpPost("complete")]
    [ProducesResponseType(typeof(CompletedHomework), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<CompletedHomework>> CompleteHomework([FromBody] CompleteHomeworkRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        var command = _mapper.Map<CompleteHomeworkCommand>(model);
        var completedHomework = await _mediator.Send(command);

        return completedHomework;
    }

    /// <summary>
    /// Check completed homework (set grade and comment)
    /// </summary>
    /// 
    /// <param name="model">Checking homework model</param>
    /// <returns>Checked completed homework</returns>
    ///
    /// <response code="200">Success. Homework checked and returned</response>
    /// <response code="404">Completed homework with given id not found (homework_not_found)</response>
    /// <response code="423">Completed homework with given id was added by another teacher, checking is denied (completed_homework_invalid_teacher)</response>
    /// <response code="422">Invalid requestModel</response>
    [HttpPost("check-сompleted")]
    [ProducesResponseType(typeof(CompletedHomework), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status423Locked)]
    public async Task<ActionResult<CompletedHomework>> CheckHomework([FromBody] CheckHomeworkRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        var command = _mapper.Map<CheckHomeworkCommand>(model);
        var completedHomework = await _mediator.Send(command);

        return completedHomework;
    }

    /// <summary>
    /// Get full information about student's completed homeworks
    /// </summary>
    /// 
    /// <param name="studentId">Student id</param>
    /// <returns>List of completed homeworks full data</returns>
    ///
    /// <response code="200">Success. Completed homeworks full data by student returned</response>
    /// <response code="404">Completed homeworks of student with given id not found (completed_homeworks_not_found)</response>
    [HttpGet("full-completed/{studentId:int}")]
    [ProducesResponseType(typeof(FullCompletedHomeworksListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullCompletedHomeworksListModel>> GetFullCompletedHomeworksByStudentId(int studentId)
    {
        var query = new GetFullCompletedHomeworksByStudentIdQuery { StudentId = studentId };
        var completedHomeworks = await _mediator.Send(query);

        return completedHomeworks;
    }

    /// <summary>
    /// Get student's uncompleted homeworks
    /// </summary>
    /// 
    /// <param name="studentId">Student id</param>
    /// <param name="groupId">Student's group id</param>
    /// <returns>List of uncompleted homeworks as public datas</returns>
    ///
    /// <response code="200">Success. Uncompleted homeworks full data by student returned</response>
    /// <response code="404">Uncompleted homeworks of student with given id not found (uncompleted_homeworks_not_found)</response>
    [HttpGet("uncompleted-homeworks/{studentId:int}/{groupId:int}")]
    [ProducesResponseType(typeof(FullCompletedHomeworksListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UncompletedHomeworksPublicListModel>> GetUncompletedHomeworksByStudent(int studentId,
        int groupId)
    {
        var query = new GetUncompletedHomeworksByStudentQuery { StudentId = studentId, GroupId = groupId };
        var uncompletedHomeworks = await _mediator.Send(query);

        return uncompletedHomeworks;
    }
}
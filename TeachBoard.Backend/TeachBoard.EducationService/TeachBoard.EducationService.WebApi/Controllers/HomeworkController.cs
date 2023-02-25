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
    [HttpPost("create")]
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
    [HttpGet("getByGroupId/{groupId:int}")]
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
    [HttpPost("complete")]
    public async Task<ActionResult<CompletedHomework>> CompleteHomework([FromBody] CompleteHomeworkRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        var command = _mapper.Map<CompleteHomeworkCommand>(model);
        var completedHomework = await _mediator.Send(command);

        return completedHomework;
    }
    
    
}
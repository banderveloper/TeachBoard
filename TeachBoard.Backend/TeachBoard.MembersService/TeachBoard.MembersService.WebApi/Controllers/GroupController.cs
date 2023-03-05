using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.MembersService.Application.Features.Groups;
using TeachBoard.MembersService.Application.Features.Students;
using TeachBoard.MembersService.Application.Validation;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.WebApi.ActionResults;
using TeachBoard.MembersService.WebApi.Models.Group;
using TeachBoard.MembersService.WebApi.Models.Validation;

namespace TeachBoard.MembersService.WebApi.Controllers;

[ValidateModel]
[ApiController]
[Route("groups")]
[Produces("application/json")]
public class GroupController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public GroupController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all groups
    /// </summary>
    /// <response code="200">Success</response>
    [HttpGet]
    [ProducesResponseType(typeof(IList<Group>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<Group>>> GetAll()
    {
        var query = new GetAllGroupsQuery();
        var groups = await _mediator.Send(query);

        return new WebApiResult(groups);
    }

    /// <summary>
    /// Get group by id
    /// </summary>
    /// <param name="id">Group id</param>
    /// <response code="200">Success</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Group), StatusCodes.Status200OK)]
    public async Task<ActionResult<Group>> GetById(int id)
    {
        var query = new GetGroupByIdQuery { GroupId = id };
        var group = await _mediator.Send(query);

        return new WebApiResult(group);
    }

    /// <summary>
    /// Get group by name
    /// </summary>
    /// <param name="name">Group name</param>
    /// <response code="200">Success</response>
    [HttpGet("by-name/{name}")]
    [ProducesResponseType(typeof(Group), StatusCodes.Status200OK)]
    public async Task<ActionResult<Group>> GetByName(string name)
    {
        var query = new GetGroupByNameQuery { GroupName = name };
        var group = await _mediator.Send(query);

        return new WebApiResult(group);
    }

    /// <summary>
    /// Create group
    /// </summary>
    /// <param name="model">New group data</param>
    /// <response code="200">Success / group_already_exists</response>
    /// <response code="422">Invalid model</response>
    [HttpPost]
    [ProducesResponseType(typeof(Group), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<Group>> Create([FromBody] CreateGroupRequestModel model)
    {
        var command = new CreateGroupCommand { Name = model.Name };
        var group = await _mediator.Send(command);

        return new WebApiResult(group);
    }
    
    /// <summary>
    /// Get student group by user id
    /// </summary>
    /// <param name="userId">User id</param>
    /// <response code="200">Success</response>
    [HttpGet("by-user/{userId:int}")]
    [ProducesResponseType(typeof(Group), StatusCodes.Status200OK)]
    public async Task<ActionResult<Group>> GetStudentGroupByUserId(int userId)
    {
        var query = new GetStudentGroupByUserIdQuery { UserId = userId };
        var group = await _mediator.Send(query);

        return new WebApiResult(group);
    }
}
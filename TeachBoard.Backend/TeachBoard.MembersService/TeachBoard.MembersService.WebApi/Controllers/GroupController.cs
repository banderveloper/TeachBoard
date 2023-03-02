using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Features.Groups;
using TeachBoard.MembersService.Application.Features.Students;
using TeachBoard.MembersService.Application.Validation;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.WebApi.Models.Group;

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
    /// <response code="200">Success. Groups returns</response>
    /// <response code="404">Groups not found (groups_not_found)</response>
    [HttpGet]
    [ProducesResponseType(typeof(GroupsListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GroupsListModel>> GetAll()
    {
        var query = new GetAllGroupsQuery();
        var groupsModel = await _mediator.Send(query);

        return Ok(groupsModel);
    }

    /// <summary>
    /// Get group by id
    /// </summary>
    /// <response code="200">Success. Group returns</response>
    /// <response code="404">Group with given id not found (group_not_found)</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Group), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Group>> GetById(int id)
    {
        var query = new GetGroupByIdQuery { GroupId = id };
        var group = await _mediator.Send(query);

        return Ok(group);
    }

    /// <summary>
    /// Get group by name
    /// </summary>
    /// <response code="200">Success. Group returns</response>
    /// <response code="404">Group with given name not found (group_not_found)</response>
    [HttpGet("by-name/{name}")]
    [ProducesResponseType(typeof(Group), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Group>> GetByName(string name)
    {
        var query = new GetGroupByNameQuery { GroupName = name };
        var group = await _mediator.Send(query);

        return Ok(group);
    }

    /// <summary>
    /// Create group
    /// </summary>
    /// <param name="model">Group creation model</param>
    /// <response code="200">Success. Group created</response>
    /// <response code="409">Group with given name already exists (group_already_exists)</response>
    [HttpPost]
    [ProducesResponseType(typeof(Group), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Group>> Create([FromBody] CreateGroupRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        var command = new CreateGroupCommand { Name = model.Name };
        var group = await _mediator.Send(command);

        return Ok(group);
    }
    
    /// <summary>
    /// Get student group by user id
    /// </summary>
    /// <param name="userId">User id</param>
    /// <response code="200">Success. Student group returns</response>
    /// <response code="404">Student with given user id not found (student_not_found) / Student does not belong to any group (group_not_found)</response>
    [HttpGet("by-user/{userId:int}")]
    [ProducesResponseType(typeof(Group), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Group>> GetStudentGroupByUserId(int userId)
    {
        var query = new GetStudentGroupByUserIdQuery { UserId = userId };
        var group = await _mediator.Send(query);

        return group;
    }
}
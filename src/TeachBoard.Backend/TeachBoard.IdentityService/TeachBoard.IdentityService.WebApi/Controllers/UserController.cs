using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.IdentityService.Application.Features.Commands;
using TeachBoard.IdentityService.Application.Features.Queries;
using TeachBoard.IdentityService.Application.Features.Queries.Common;
using TeachBoard.IdentityService.WebApi.ActionResults;
using TeachBoard.IdentityService.WebApi.Models.User;
using TeachBoard.IdentityService.WebApi.Validation;

namespace TeachBoard.IdentityService.WebApi.Controllers;

[ApiController]
[Route("user")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Create new pending user
    /// </summary>
    /// 
    /// <param name="model">Personal data of the creating user</param>
    /// <returns>Register code and expiration date</returns>
    ///
    /// <response code="200">Success / email_already_exists / phoneNumber_already_exists</response>
    /// <response code="422">Invalid model state</response>
    [HttpPost("pending")]
    [ProducesResponseType(typeof(RegisterCodeModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<RegisterCodeModel>> CreatePendingUser(
        [FromBody] CreatePendingUserRequestModel model)
    {
        var command = _mapper.Map<CreatePendingUserCommand>(model);

        // Send command which create pending user and return register code and expiration date
        var registerCodeModel = await _mediator.Send(command);

        return new WebApiResult(registerCodeModel);
    }

    /// <summary>
    /// Approve pending user
    /// </summary>
    /// 
    /// <param name="model">Register code and user credentials</param>
    ///
    /// <response code="200">Success / pending_user_not_found / pending_user_expired / user_already_exists</response>
    /// <response code="422">Invalid model state</response>
    [HttpPost("pending/approve")]
    [ProducesResponseType(typeof(UserPublicDataResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<UserPublicDataResponseModel>> ApprovePendingUser(
        [FromBody] ApprovePendingUserRequestModel model)
    {
        // If everything is ok - it will be created user, pending user will be deleted
        var approveCommand = _mapper.Map<ApprovePendingUserCommand>(model);
        var user = await _mediator.Send(approveCommand);

        var response = _mapper.Map<UserPublicDataResponseModel>(user);

        return new WebApiResult(response);
    }

    /// <summary>
    /// Get user public data by id
    /// </summary>
    /// 
    /// <param name="userId">User id</param>
    ///
    /// <response code="200">Success. User public data returned</response>
    [HttpGet("{userId:int}")]
    [ProducesResponseType(typeof(UserPublicDataResponseModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserPublicDataResponseModel>> GetUserById(int userId)
    {
        var query = new GetUserByIdQuery { UserId = userId };

        var user = await _mediator.Send(query);
        var responseModel = _mapper.Map<UserPublicDataResponseModel>(user);

        return new WebApiResult(responseModel);
    }

    /// <summary>
    /// Get users ids, names and photos by user ids
    /// </summary>
    /// 
    /// <param name="userIds">Users ids</param>
    ///
    /// <response code="200">Success. Users ids, names and photos returned</response>
    [HttpGet("presentation")]
    [ProducesResponseType(typeof(IList<UserPresentationDataModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<UserPresentationDataModel>>> GetUserPresentationDataModels(
        [FromQuery] List<int> userIds)
    {
        var query = new GetUsersPresentationDataByIdsQuery { Ids = userIds };
        var usersModel = await _mediator.Send(query);

        return new WebApiResult(usersModel);
    }

    /// <summary>
    /// Get users ids, names and photos by partial name
    /// </summary>
    /// 
    /// <param name="partialName">Partial user's name, minimal length is 3</param>
    ///
    /// <response code="200">Success. Users ids, names and photos returned</response>
    [HttpGet("presentation/{partialName}")]
    [ProducesResponseType(typeof(IList<UserPresentationDataModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<UserPresentationDataModel>>> GetUserPresentationDatModelsByPartialName(
        string partialName)
    {
        var query = new GetUsersPresentationDataByPartialNameQuery { PartialName = partialName };
        var users = await _mediator.Send(query);

        return new WebApiResult(users);
    }

    /// <summary>
    /// Update user public data
    /// </summary>
    /// 
    /// <param name="model">New user public data</param>
    ///
    /// <response code="200">Success / user_not_found</response>
    [HttpPut]
    [ProducesResponseType(typeof(UserPresentationDataModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserPublicDataResponseModel>> UpdateUser([FromBody] UpdateUserRequestModel model)
    {
        var command = _mapper.Map<UpdateUserCommand>(model);
        var updatedUser = await _mediator.Send(command);

        var userPublicData = _mapper.Map<UserPublicDataResponseModel>(updatedUser);

        return new WebApiResult(userPublicData);
    }
}
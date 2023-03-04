using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.IdentityService.Application.CQRS.Commands;
using TeachBoard.IdentityService.Application.CQRS.Queries;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Domain.Entities;
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
    /// <param name="model">Create pending user requestModel with personal data of user</param>
    /// <returns>Register code and expiration date</returns>
    ///
    /// <response code="200">Success. Pending user created</response>
    /// <response code="200">Pending user with given phone/email already exists (phone_already_exists / email_already_exists)</response>
    /// <response code="422">Invalid requestModel</response>
    [HttpPost("pending")]
    [ProducesResponseType(typeof(RegisterCodeModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<RegisterCodeModel>> CreatePendingUser(
        [FromBody] CreatePendingUserRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        var command = _mapper.Map<CreatePendingUserCommand>(model);

        // Send command which create pending user and return register code and expiration date
        var registerCodeModel = await _mediator.Send(command);

        return new WebApiResult(registerCodeModel);
    }

    /// <summary>
    /// Approve pending user
    /// </summary>
    /// 
    /// <param name="requestModel">Approve pending user created by administrator</param>
    ///
    /// <response code="200">i love ayrat</response>
    /// <response code="200">username_already_exists</response>
    /// <response code="200">ty_dolbaeb</response>
    [HttpPost("pending/approve")]
    [ProducesResponseType(typeof(WebApiResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ApprovePendingUser([FromBody] ApprovePendingUserRequestModel requestModel)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(requestModel);

        // Create a command for approve and send it
        // If everything is ok - it will be created user, pending user will be deleted
        var approveCommand = _mapper.Map<ApprovePendingUserCommand>(requestModel);
        var user = await _mediator.Send(approveCommand);

        return new WebApiResult(user);
    }

    /// <summary>
    /// Get user public data by id
    /// </summary>
    /// 
    /// <param name="id">User id</param>
    ///
    /// <response code="200">Success. User public data returned</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UserPublicDataResponseModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserPublicDataResponseModel>> GetUserById(int id)
    {
        var query = new GetUserByIdQuery { UserId = id };

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
    public async Task<ActionResult<IList<UserPresentationDataModel>>> GetUsersNamesPhotosByIds(
        [FromQuery] List<int> userIds)
    {
        var query = new GetUsersPresentationDataByIdsQuery { Ids = userIds };
        var usersModel = await _mediator.Send(query);

        return new WebApiResult(usersModel);
    }
}
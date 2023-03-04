using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.IdentityService.Application.CQRS.Commands;
using TeachBoard.IdentityService.Application.CQRS.Queries;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Domain.Entities;
using TeachBoard.IdentityService.WebApi.Models.User;
using TeachBoard.IdentityService.WebApi.Models.Validation;

namespace TeachBoard.IdentityService.WebApi.Controllers;

[ApiController]
[Route("user")]
[Produces("application/json")]
[ValidateModel]
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
    /// <param name="requestModel">Create pending user requestModel with personal data of user</param>
    /// <returns>Register code and expiration date</returns>
    ///
    /// <response code="200">Success. Pending user created</response>
    /// <response code="409">Pending user with given phone/email already exists (phone_already_exists / email_already_exists)</response>
    /// <response code="422">Invalid requestModel</response>
    [HttpPost("pending")]
    [ProducesResponseType(typeof(RegisterCodeModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<RegisterCodeModel>> CreatePendingUser(
        [FromBody] CreatePendingUserRequestModel requestModel)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(requestModel);

        var command = _mapper.Map<CreatePendingUserCommand>(requestModel);

        // Send command which create pending user and return register code and expiration date
        var registerCodeModel = await _mediator.Send(command);

        return Ok(registerCodeModel);
    }

    /// <summary>
    /// Approve pending user
    /// </summary>
    /// 
    /// <param name="requestModel">Approve pending user created by administrator</param>
    ///
    /// <response code="200">Success. User approved and returned</response>
    /// <response code="404">Pending user with given register code not found (register_code_not_found)</response>
    /// <response code="409">User with given username already exists (username_already_exists)</response>
    /// <response code="410">Pending user expired (pending_user_expired)</response>
    /// <response code="422">Invalid requestModel</response>
    [HttpPost("pending/approve")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ApprovePendingUser([FromBody] ApprovePendingUserRequestModel requestModel)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(requestModel);

        // Create a command for approve and send it
        // If everything is ok - it will be created user, pending user will be deleted
        var approveCommand = _mapper.Map<ApprovePendingUserCommand>(requestModel);
        var user = await _mediator.Send(approveCommand);

        return Ok(user);
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
        var responseModel = _mapper.Map<UserPublicDataResponseModel>(user) ?? new object();

        return Ok(responseModel);
    }

    /// <summary>
    /// Get pending user role
    /// </summary>
    /// 
    /// <param name="registerCode">Register code of pending user</param>
    ///
    /// <response code="200">Success. Pending user role returned</response>
    [HttpGet("pending/role/{registerCode}")]
    [ProducesResponseType(typeof(PendingUserRoleResponseModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<PendingUserRoleResponseModel>> GetPendingUserRoleByCode(string registerCode)
    {
        var query = new GetPendingUserByRegistrationCodeQuery { RegisterCode = registerCode };
        var pendingUser = await _mediator.Send(query);

        return Ok(pendingUser is not null
            ? new PendingUserRoleResponseModel { Role = pendingUser.Role }
            : new object()
        );
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

        return Ok(usersModel);
    }
}
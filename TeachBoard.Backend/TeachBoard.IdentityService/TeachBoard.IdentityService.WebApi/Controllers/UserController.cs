using System.Text.Json;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.IdentityService.Application.CQRS.Commands.ApprovePendingUser;
using TeachBoard.IdentityService.Application.CQRS.Commands.CreatePendingUser;
using TeachBoard.IdentityService.Application.CQRS.Queries.GetPendingUserByRegisterCode;
using TeachBoard.IdentityService.Application.CQRS.Queries.GetUserById;
using TeachBoard.IdentityService.Application.CQRS.Queries.GetUserNamesPhotosByIds;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Domain.Entities;
using TeachBoard.IdentityService.WebApi.Models.User;
using TeachBoard.IdentityService.WebApi.Models.Validation;

namespace TeachBoard.IdentityService.WebApi.Controllers;

[ApiController]
[Route("users")]
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
    [HttpPost("pending/create")]
    [ProducesResponseType(typeof(RegisterCodeModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<RegisterCodeModel>> CreatePendingUser(
        [FromBody] CreatePendingUserRequestModel requestModel)
    {
        Console.WriteLine("Income create pending user model:");
        Console.WriteLine(JsonSerializer.Serialize(requestModel));
        
        if (!ModelState.IsValid)
            return UnprocessableEntity(requestModel);

        var command = _mapper.Map<CreatePendingUserCommand>(requestModel);

        // Send command which create pending user and return register code and expiration date
        var registerCodeModel = await _mediator.Send(command);

        Console.WriteLine("Register code output model:");
        Console.WriteLine(JsonSerializer.Serialize(registerCodeModel));

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
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status410Gone)]
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
    /// <response code="404">User with given id not found (user_not_found)</response>
    [HttpGet("getbyid/{id:int}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserPublicDataResponseModel>> GetUserById(int id)
    {
        var query = new GetUserByIdQuery { UserId = id };

        var user = await _mediator.Send(query);
        var publicModel = _mapper.Map<UserPublicDataResponseModel>(user);

        return Ok(publicModel);
    }

    /// <summary>
    /// Get pending user role
    /// </summary>
    /// 
    /// <param name="registerCode">Register code of pending user</param>
    ///
    /// <response code="200">Success. Pending user role returned</response>
    /// <response code="404">Pending user with given register code not found (pending_user_not_found)</response>
    [HttpGet("pending/getrolebycode/{registerCode}")]
    [ProducesResponseType(typeof(PendingUserRoleResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PendingUserRoleResponseModel>> GetPendingUserRoleByCode(string registerCode)
    {
        var query = new GetPendingUserByRegistrationCodeQuery { RegisterCode = registerCode };
        var pendingUser = await _mediator.Send(query);

        return new PendingUserRoleResponseModel
        {
            Role = pendingUser.Role
        };
    }

    /// <summary>
    /// Get users names and photos by user ids
    /// </summary>
    /// 
    /// <param name="ids">Users ids</param>
    ///
    /// <response code="200">Success. Users names and photos returned</response>
    /// <response code="404">Users with given ids not found (users_not_found)</response>
    [HttpGet("getNamesPhotosByIds")]
    [ProducesResponseType(typeof(UsersNamePhotoListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsersNamePhotoListModel>> GetUsersNamesPhotosByIds([FromBody] List<int> ids)
    {
        var query = new GetUserNamesPhotosByIdsQuery { Ids = ids };
        var usersModel = await _mediator.Send(query);

        return usersModel;
    }
    
}
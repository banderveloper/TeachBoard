using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.Gateway.Application.Exceptions;
using TeachBoard.Gateway.Application.Models.Education.Response;
using TeachBoard.Gateway.Application.Models.Identity.Request;
using TeachBoard.Gateway.Application.Models.Identity.Response;
using TeachBoard.Gateway.Application.Models.Members.Request;
using TeachBoard.Gateway.Application.RefitClients;
using TeachBoard.Gateway.Application.Validation;
using TeachBoard.Gateway.WebApi.Models;

namespace TeachBoard.Gateway.WebApi.Controllers;

[Route("api/student")]
[Authorize(Roles = "Student")]
public class StudentController : BaseController
{
    private readonly IIdentityClient _identityClient;
    private readonly IMembersClient _membersClient;
    private readonly IEducationClient _educationClient;

    public StudentController(IIdentityClient identityClient, IMembersClient membersClient,
        IEducationClient educationClient)
    {
        _identityClient = identityClient;
        _membersClient = membersClient;
        _educationClient = educationClient;
    }

    /// <summary>
    /// Approve pending student
    /// </summary>
    /// 
    /// <param name="model">Approve pending student model with needed register code, new username and password</param>
    /// <returns>None</returns>
    ///
    /// <response code="200">Success. Pending student approved</response>
    /// <response code="404">Pending user with given register code not found (register_code_not_found)</response>
    /// <response code="409">User with given username already exists (username_already_exists)</response>
    /// <response code="410">Pending user expired (pending_user_expired)</response>
    /// <response code="422">Invalid model</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [AllowAnonymous]
    [HttpPost("approvePending")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status410Gone)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ApproveStudent([FromBody] ApprovePendingUserRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        var user = await _identityClient.ApprovePendingUser(model);
        await _membersClient.CreateStudent(new CreateStudentRequestModel { UserId = user.Id });

        return Ok();
    }

    /// <summary>
    /// Get student group members with fields: id, name, surname, patronymic, avatarImagePath 
    /// </summary>
    ///
    /// <remarks>Requires JWT-token with user id, binded to student</remarks>
    ///
    /// <response code="200">Success. Student group members list returned</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">
    /// Users with given ids not found (users_not_found) /
    /// Student with given user id not found (student_not_found) /
    /// Student does not belong to any group (group_not_found)
    /// </response>
    /// <response code="406">Jwt-token does not contains user id (jwt_user_id_not_found)</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("getGroupMembers")]
    [ProducesResponseType(typeof(UsersNamePhotoListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<UsersNamePhotoListModel>> GetStudentGroupMembers()
    {
        // Get student group members user ids
        var studentsListModel = await _membersClient.GetStudentGroupMembersByUserId(UserId);
        var studentUserIds = studentsListModel.Students.Select(student => student.UserId).ToList();

        // Get their names and photos
        var usersModel = await _identityClient.GetUserNamesPhotosByIds(studentUserIds);

        return Ok(usersModel);
    }

    /// <summary>
    /// Get all student lessons 
    /// </summary>
    ///
    /// <remarks>Requires JWT-token with user id, binded to student</remarks>
    ///
    /// <response code="200">Success. Student lessons list returned</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">
    /// Student with given user id not found (student_not_found) /
    /// Student does not belong to any group (group_not_found) /
    /// Lessons not found (lessons_not_found)
    /// </response>
    /// <response code="406">Jwt-token does not contains user id (jwt_user_id_not_found)</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("getAllLessons")]
    [ProducesResponseType(typeof(LessonsListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<LessonsListModel>> GetStudentLessons()
    {
        var studentGroup = await _membersClient.GetStudentGroupByUserId(UserId);
        var groupLessons = await _educationClient.GetLessonsByGroupId(studentGroup.Id);

        return groupLessons;
    }

    /// <summary>
    /// Get student public data, containing user and group data
    /// </summary>
    ///
    /// <remarks>Requires JWT-token with user id, binded to student</remarks>
    ///
    /// <response code="200">Success. Student's user and group data returned</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">
    /// User with given user id not found (user_not_found) /
    /// Student does not belong to any group (group_not_found) /
    /// Student with given user id not found (student_not_found)
    /// </response>
    /// <response code="406">Jwt-token does not contains user id (jwt_user_id_not_found)</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("getProfileData")]
    [ProducesResponseType(typeof(UserProfileDataResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<UserProfileDataResponseModel>> GetStudentProfileData()
    {
        var userData = await _identityClient.GetUserById(UserId);
        var userGroup = await _membersClient.GetStudentGroupByUserId(UserId);

        return new UserProfileDataResponseModel
        {
            User = userData,
            Group = userGroup
        };
    }

    /// <summary>
    /// Get student examinations activities
    /// </summary>
    ///
    /// <remarks>Requires JWT-token with user id, binded to student</remarks>
    ///
    /// <response code="200">Success. Student's examinations activites returned</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">
    /// Student with given user id not found (student_not_found) /
    /// Student examination activies with given id student id not found (student_examination_activities_not_found) /
    /// </response>
    /// <response code="406">Jwt-token does not contains user id (jwt_user_id_not_found)</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("getExaminationsActivities")]
    [ProducesResponseType(typeof(StudentExaminationsPublicDataListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<StudentExaminationsPublicDataListModel>> GetStudentExaminationsPublicData()
    {
        var studentInfo = await _membersClient.GetStudentByUserId(UserId);
        var examinations = await _educationClient.GetStudentExaminationsPublicData(studentInfo.Id);

        return examinations;
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.Gateway.Application.Refit.Clients;
using TeachBoard.Gateway.Application.Refit.RequestModels.Identity;
using TeachBoard.Gateway.Application.Refit.RequestModels.Members;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Education;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Identity;
using TeachBoard.Gateway.WebApi.ActionResults;
using TeachBoard.Gateway.WebApi.Models;

namespace TeachBoard.Gateway.WebApi.Controllers;

[Route("api/student")]
[Authorize(Roles = "Student")]
public class StudentController : BaseController
{
    private readonly IIdentityClient _identityClient;
    private readonly IMembersClient _membersClient;
    private readonly IEducationClient _educationClient;

    private ILogger<StudentController> _logger;

    public StudentController(IIdentityClient identityClient, IMembersClient membersClient, IEducationClient educationClient, ILogger<StudentController> logger)
    {
        _identityClient = identityClient;
        _membersClient = membersClient;
        _educationClient = educationClient;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("approve-pending")]
    public async Task<IActionResult> ApproveStudent([FromBody] ApprovePendingUserRequestModel model)
    {
        var identityResponse = await _identityClient.ApprovePendingUser(model);
        var user = identityResponse.Data;

        await _membersClient.CreateStudent(new CreateStudentRequestModel { UserId = user.Id });

        return new WebApiResult();
    }

    [HttpGet("group-members")]
    public async Task<ActionResult<IList<UserPresentationDataModel>>> GetStudentGroupMembers()
    {
        // Get student group members user ids
        var membersResponse = await _membersClient.GetStudentGroupMembersByUserId(UserId);
        var studentUserIds = membersResponse.Data?.Select(student => student.UserId).ToList();

        // Get users presentations model with name and photo
        var identityResponse = await _identityClient.GetUserPresentationDataModels(studentUserIds);
        var usersPresentationsList = identityResponse.Data;

        return new WebApiResult(usersPresentationsList);
    }

    // [HttpGet("all-lessons")]
    // public async Task<ActionResult<IList<Lesson>>> GetStudentLessons()
    // {
    //     var studentGroup = await _membersClient.GetStudentGroupByUserId(UserId);
    //     var groupLessons = await _educationClient.GetLessonsByGroupId(studentGroup.Id);
    //
    //     return groupLessons;
    // }

    [HttpGet("profile-data")]
    public async Task<ActionResult<UserProfileDataResponseModel>> GetStudentProfileData()
    {
        // Get user public data
        var identityResponse = await _identityClient.GetUserPublicData(UserId);
        var userPublicData = identityResponse.Data;

        // Get user group
        var membersResponse = await _membersClient.GetStudentGroupByUserId(UserId);
        var userGroup = membersResponse.Data;

        // union user data and his group
        var response = new UserProfileDataResponseModel
        {
            User = userPublicData,
            Group = userGroup
        };

        return new WebApiResult(response);
    }

    [HttpGet("exam-activities")]
    public async Task<ActionResult<IList<StudentExaminationActivityPresentationDataModel>>> GetStudentExaminationsPublicData()
    {
        var membersResponse = await _membersClient.GetStudentByUserId(UserId);
        var student = membersResponse.Data;
        
        if(student?.Id == 0)
            _logger.LogWarning($"GetStudentExaminationsPublicData warning. User id from token is [{UserId}], but student id is 0");
        
        var educationResponse = await _educationClient.GetStudentExaminationsActivities(student.Id);
        var examinations = educationResponse.Data;

        return new WebApiResult(examinations);
    }
    //
    // /// <summary>
    // /// Get student's completed homeworks
    // /// </summary>
    // ///
    // /// <remarks>Requires JWT-token with user id, binded to student</remarks>
    // ///
    // /// <response code="200">Success. Student's completed homeworks returned</response>
    // /// <response code="401">Unauthorized</response>
    // /// <response code="404">
    // /// Student with given user id not found (student_not_found) /
    // /// Completed homeworks with given student id not found (completed_homeworks_not_found) /
    // /// </response>
    // /// <response code="406">Jwt-token does not contains user id (jwt_user_id_not_found)</response>
    // /// <response code="503">One of the needed services is unavailable now</response>
    // [HttpGet("completed-homeworks")]
    // [ProducesResponseType(typeof(FullCompletedHomeworksListModel), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    // [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    // [ProducesResponseType(typeof(IApiException), StatusCodes.Status406NotAcceptable)]
    // [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    // public async Task<ActionResult<FullCompletedHomeworksListModel>> GetCompletedHomeworks()
    // {
    //     var studentInfo = await _membersClient.GetStudentByUserId(UserId);
    //     var completedHomeworks = await _educationClient.GetStudentFullCompletedHomeworks(studentInfo.Id);
    //
    //     return completedHomeworks;
    // }
    //
    // /// <summary>
    // /// Get student's uncompleted homeworks
    // /// </summary>
    // ///
    // /// <remarks>Requires JWT-token with user id, binded to student</remarks>
    // ///
    // /// <response code="200">Success. Student's uncompleted homeworks returned</response>
    // /// <response code="401">Unauthorized</response>
    // /// <response code="404">
    // /// Student with given user id not found (student_not_found) /
    // /// Uncompleted homeworks with given student id not found (uncompleted_homeworks_not_found) /
    // /// </response>
    // /// <response code="406">Jwt-token does not contains user id (jwt_user_id_not_found)</response>
    // /// <response code="503">One of the needed services is unavailable now</response>
    // [HttpGet("uncompleted-homeworks")]
    // [ProducesResponseType(typeof(UncompletedHomeworksPublicListModel), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    // [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    // [ProducesResponseType(typeof(IApiException), StatusCodes.Status406NotAcceptable)]
    // [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    // public async Task<ActionResult<UncompletedHomeworksPublicListModel>> GetUncompletedHomeworks()
    // {
    //     var studentInfo = await _membersClient.GetStudentByUserId(UserId);
    //
    //     var uncompletedHomeworks = await _educationClient.GetStudentUncompletedHomeworks(
    //         studentId: studentInfo.Id,
    //         groupId: studentInfo.GroupId
    //     );
    //
    //     return uncompletedHomeworks;
    // }
    //
    // /// <summary>
    // /// Get all student's lesson activities
    // /// </summary>
    // ///
    // /// <remarks>Requires JWT-token with user id, binded to student</remarks>
    // ///
    // /// <response code="200">Success. Student's lessons activities returned</response>
    // /// <response code="401">Unauthorized</response>
    // /// <response code="404">
    // /// Student with given user id not found (student_not_found) /
    // /// Student's lesson activities (student_lesson_activities_not_found) /
    // /// </response>
    // /// <response code="406">Jwt-token does not contains user id (jwt_user_id_not_found)</response>
    // /// <response code="503">One of the needed services is unavailable now</response>
    // [HttpGet("lessons-activities")]
    // [ProducesResponseType(typeof(StudentLessonActivityPublicListModel), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    // [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    // [ProducesResponseType(typeof(IApiException), StatusCodes.Status406NotAcceptable)]
    // [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    // public async Task<ActionResult<StudentLessonActivityPublicListModel>> GetLessonsActivities()
    // {
    //     var studentInfo = await _membersClient.GetStudentByUserId(UserId);
    //
    //     var lessonsActivities = await _educationClient.GetStudentLessonActivities(studentInfo.Id);
    //     return lessonsActivities;
    // }
    //
    // /// <summary>
    // /// Create completed homework
    // /// </summary>
    // ///
    // /// <remarks>Requires JWT-token with user id, binded to student</remarks>
    // ///
    // /// <response code="200">Success. Completed homework created</response>
    // /// <response code="401">Unauthorized</response>
    // /// <response code="404">
    // /// Student with given user id not found (student_not_found) /
    // /// Homework with given id not found (homework_not_found)
    // /// Homework with given id to given group not found (homework_not_found)
    // /// </response>
    // /// <response code="406">Jwt-token does not contains user id (jwt_user_id_not_found)</response>
    // /// <response code="409">Student already completed this homework (completed_homework_already_exists)</response>
    // /// <response code="503">One of the needed services is unavailable now</response>
    // [HttpPost("complete-homework")]
    // [ProducesResponseType(typeof(StudentLessonActivityPublicListModel), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    // [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    // [ProducesResponseType(typeof(IApiException), StatusCodes.Status406NotAcceptable)]
    // [ProducesResponseType(typeof(IApiException), StatusCodes.Status409Conflict)]
    // [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    // public async Task<ActionResult<CompletedHomework>> CompleteHomework([FromBody] CreateCompletedHomeworkRequestModel model)
    // {
    //     var studentInfo = await _membersClient.GetStudentByUserId(UserId);
    //
    //     var completeRequest = new CompleteHomeworkRequestModel
    //     {
    //         StudentId = studentInfo.Id,
    //         StudentGroupId = studentInfo.GroupId,
    //         HomeworkId = model.HomeworkId,
    //         StudentComment = model.StudentComment,
    //         FilePath = model.FilePath
    //     };
    //     
    //     return await _educationClient.CompleteHomework(completeRequest);
    // }
}
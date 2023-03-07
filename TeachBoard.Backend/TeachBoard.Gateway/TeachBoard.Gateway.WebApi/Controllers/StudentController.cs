using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.Gateway.Application.Refit.Clients;
using TeachBoard.Gateway.Application.Refit.RequestModels.Education;
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

    private readonly ILogger<StudentController> _logger;

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
        
        var educationResponse = await _educationClient.GetExaminationsActivities(student.Id);
        var examinations = educationResponse.Data;

        return new WebApiResult(examinations);
    }
    
    [HttpGet("completed-homeworks")]
    public async Task<ActionResult<IList<CompletedHomeworkPresentationDataModel>>> GetCompletedHomeworks()
    {
        var membersResponse = await _membersClient.GetStudentByUserId(UserId);
        var student = membersResponse.Data;
        
        if(student?.Id == 0)
            _logger.LogWarning($"GetCompletedHomeworks warning. User id from token is [{UserId}], but student id is 0");
        
        var educationResponse = await _educationClient.GetCompletedHomeworks(student.Id);
        var completedHomeworks = educationResponse.Data;
    
        return new WebApiResult(completedHomeworks);
    }
    
    [HttpGet("uncompleted-homeworks")]
    public async Task<ActionResult<IList<UncompletedHomeworkPresentationDataModel>>> GetUncompletedHomeworks()
    {
        var membersResponse = await _membersClient.GetStudentByUserId(UserId);
        var student = membersResponse.Data;
        
        if(student?.Id == 0)
            _logger.LogWarning($"GetUncompletedHomeworks warning. User id from token is [{UserId}], but student id is 0");
    
        var educationResponse = await _educationClient.GetUncompletedHomeworks(student.Id, student.GroupId);
        var uncompletedHomeworks = educationResponse.Data;
    
        return new WebApiResult(uncompletedHomeworks);
    }
    
    [HttpGet("lessons-activities")]
    public async Task<ActionResult<IList<StudentLessonActivityPresentationDataModel>>> GetLessonsActivities()
    {
        var membersResponse = await _membersClient.GetStudentByUserId(UserId);
        var student = membersResponse.Data;
        
        if(student?.Id == 0)
            _logger.LogWarning($"GetUncompletedHomeworks warning. User id from token is [{UserId}], but student id is 0");
    
        var educationResponse = await _educationClient.GetLessonsActivity(student.Id);
        var lessonsActivities = educationResponse.Data;

        return new WebApiResult(lessonsActivities);
    }
    
    [HttpPost("complete-homework")]
    public async Task<ActionResult<CompletedHomework>> CompleteHomework([FromBody] CreateCompletedHomeworkRequestModel model)
    {
        var membersResponse = await _membersClient.GetStudentByUserId(UserId);
        var student = membersResponse.Data;
    
        var completeHomeworkInternalRequest = new CompleteHomeworkInternalRequestModel
        {
            StudentId = student.Id,
            StudentGroupId = student.GroupId,
            HomeworkId = model.HomeworkId,
            StudentComment = model.StudentComment,
            FilePath = model.FilePath
        };

        var educationResponse = await _educationClient.CompleteHomework(completeHomeworkInternalRequest);
        var completedHomework = educationResponse.Data;

        return new WebApiResult(completedHomework);
    }
}
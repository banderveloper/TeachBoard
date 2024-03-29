﻿using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Refit;
using TeachBoard.Gateway.Application.Refit.Clients;
using TeachBoard.Gateway.Application.Refit.RequestModels.Education;
using TeachBoard.Gateway.Application.Refit.RequestModels.Identity;
using TeachBoard.Gateway.Application.Refit.RequestModels.Members;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Education;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Identity;
using TeachBoard.Gateway.Application.Validation;
using TeachBoard.Gateway.WebApi.ActionResults;
using TeachBoard.Gateway.WebApi.Models;

namespace TeachBoard.Gateway.WebApi.Controllers;

[Route("api/student")]
[Microsoft.AspNetCore.Authorization.Authorize(Roles = "Student")]
public class StudentController : BaseController
{
    private readonly IIdentityClient _identityClient;
    private readonly IMembersClient _membersClient;
    private readonly IEducationClient _educationClient;
    private readonly IFilesClient _filesClient;

    private readonly ILogger<StudentController> _logger;

    public StudentController(IIdentityClient identityClient, IMembersClient membersClient,
        IEducationClient educationClient, ILogger<StudentController> logger, IFilesClient filesClient)
    {
        _identityClient = identityClient;
        _membersClient = membersClient;
        _educationClient = educationClient;
        _logger = logger;
        _filesClient = filesClient;
    }
    
    /// <summary>
    /// Approve pending user with student role
    /// </summary>
    /// 
    /// <param name="model">Register code and user credentials</param>
    ///
    /// <response code="200">
    /// Success /
    /// pending_user_not_found / pending_user_expired / user_already_exists /
    /// group_not_found / student_already_exists
    /// </response>
    /// <response code="422">Invalid model state</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [AllowAnonymous]
    [HttpPost("approve-pending")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ApproveStudent([FromBody] ApprovePendingUserRequestModel model)
    {
        // approve pending user and get it
        var identityResponse = await _identityClient.ApprovePendingUser(model);
        var user = identityResponse.Data;

        // create student entity to given user
        await _membersClient.CreateStudent(new CreateStudentRequestModel { UserId = user.Id });

        return new WebApiResult();
    }

    /// <summary>
    /// Get presentation data (id, name, avatar) of student's group members
    /// </summary>
    /// 
    /// <remarks>Requires in-header JWT-token with user id, bound to student</remarks>
    ///
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("group-members")]
    [ProducesResponseType(typeof(IList<UserPresentationDataModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IList<UserPresentationDataModel>>> GetStudentGroupMembers()
    {
        // Get student group members user ids
        var membersResponse = await _membersClient.GetStudentGroupMembersByUserId(UserId);
        var studentUserIds = membersResponse.Data?.Select(student => student.UserId).ToList();

        // Get group members users presentation models (name, avatar)
        var identityResponse = await _identityClient.GetUserPresentationDataModels(studentUserIds);
        var usersPresentationsList = identityResponse.Data;

        return new WebApiResult(usersPresentationsList);
    }

    /// <summary>
    /// Get lesson bound to student's group
    /// </summary>
    /// 
    /// <remarks>Requires in-header JWT-token with user id, bound to student</remarks>
    ///
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("all-lessons")]
    [ProducesResponseType(typeof(IList<LessonPresentationDataModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IList<LessonPresentationDataModel>>> GetStudentLessons()
    {
        var membersResponse = await _membersClient.GetStudentGroupByUserId(UserId);
        var studentGroup = membersResponse.Data;

        // get all lessons using student's group id
        var educationResponse = await _educationClient.GetGroupLessons(studentGroup.Id);
        var groupLessons = educationResponse.Data;

        return new WebApiResult(groupLessons);
    }

    /// <summary>
    /// Get student's profile data
    /// </summary>
    /// 
    /// <remarks>Requires in-header JWT-token with user id, bound to student</remarks>
    ///
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("profile-data")]
    [ProducesResponseType(typeof(UserProfileDataResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
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

    /// <summary>
    /// Get student's examinations activities
    /// </summary>
    /// 
    /// <remarks>Requires in-header JWT-token with user id, bound to student</remarks>
    ///
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("exam-activities")]
    [ProducesResponseType(typeof(IList<StudentExaminationActivityPresentationDataModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IList<StudentExaminationActivityPresentationDataModel>>>
        GetStudentExaminationsPublicData()
    {
        var membersResponse = await _membersClient.GetStudentByUserId(UserId);
        var student = membersResponse.Data;

        if (student?.Id == 0)
            _logger.LogWarning(
                $"GetStudentExaminationsPublicData warning. User id from token is [{UserId}], but student id is 0");

        var educationResponse = await _educationClient.GetExaminationsActivities(student.Id);
        var examinations = educationResponse.Data;

        return new WebApiResult(examinations);
    }

    /// <summary>
    /// Get student's completed homeworks
    /// </summary>
    /// 
    /// <remarks>Requires in-header JWT-token with user id, bound to student</remarks>
    ///
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("completed-homeworks")]
    [ProducesResponseType(typeof(IList<CompletedHomeworkPresentationDataModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IList<CompletedHomeworkPresentationDataModel>>> GetCompletedHomeworks()
    {
        var membersResponse = await _membersClient.GetStudentByUserId(UserId);
        var student = membersResponse.Data;

        if (student?.Id == 0)
            _logger.LogWarning($"GetCompletedHomeworks warning. User id from token is [{UserId}], but student id is 0");

        var educationResponse = await _educationClient.GetCompletedHomeworks(student.Id);
        var completedHomeworks = educationResponse.Data;

        return new WebApiResult(completedHomeworks);
    }

    /// <summary>
    /// Get student's uncompleted homeworks
    /// </summary>
    /// 
    /// <remarks>Requires in-header JWT-token with user id, bound to student</remarks>
    ///
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("uncompleted-homeworks")]
    [ProducesResponseType(typeof(IList<UncompletedHomeworkPresentationDataModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IList<UncompletedHomeworkPresentationDataModel>>> GetUncompletedHomeworks()
    {
        var membersResponse = await _membersClient.GetStudentByUserId(UserId);
        var student = membersResponse.Data;

        Console.WriteLine("USerID: " + UserId);
        Console.WriteLine("Student: " + JsonSerializer.Serialize(student));
        

        if (student?.Id == 0)
            _logger.LogWarning(
                $"GetUncompletedHomeworks warning. User id from token is [{UserId}], but student id is 0");

        var educationResponse = await _educationClient.GetUncompletedHomeworks(student.Id, student.GroupId);
        var uncompletedHomeworks = educationResponse.Data;

        Console.WriteLine("Uncompleted homeworks: " + uncompletedHomeworks);
        Console.WriteLine("Uncompleted homeworks: " + JsonSerializer.Serialize(uncompletedHomeworks));
        
        return new WebApiResult(uncompletedHomeworks);
    }

    /// <summary>
    /// Get student's all lessons activities
    /// </summary>
    /// 
    /// <remarks>Requires in-header JWT-token with user id, bound to student</remarks>
    ///
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("lessons-activities")]
    [ProducesResponseType(typeof(IList<StudentLessonActivityPresentationDataModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IList<StudentLessonActivityPresentationDataModel>>> GetLessonsActivities()
    {
        var membersResponse = await _membersClient.GetStudentByUserId(UserId);
        var student = membersResponse.Data;

        if (student?.Id == 0)
            _logger.LogWarning(
                $"GetUncompletedHomeworks warning. User id from token is [{UserId}], but student id is 0");

        var educationResponse = await _educationClient.GetLessonsActivity(student.Id);
        var lessonsActivities = educationResponse.Data;

        return new WebApiResult(lessonsActivities);
    }

    /// <summary>
    /// Complete given by teacher homework
    /// </summary>
    /// 
    /// <remarks>Requires in-header JWT-token with user id, bound to student</remarks>
    ///
    /// <response code="200">Success / homework_not_found / completed_homework_already_exists</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Invalid model</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpPost("complete-homework")]
    [ProducesResponseType(typeof(CompletedHomework), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<CompletedHomework>> CompleteHomework(
        [FromForm] CreateCompletedHomeworkRequestModel model)
    {
        var membersResponse = await _membersClient.GetStudentByUserId(UserId);
        var student = membersResponse.Data;

        await using var stream = model.File.OpenReadStream();
        var streamPart = new StreamPart(stream, model.File.FileName, "image/jpeg");

        var uploadFileResponse =
            await _filesClient.UploadHomeworkSolutionFile(student.Id, model.HomeworkId, streamPart);
        var uploadedFile = uploadFileResponse.Data;
        
        var completeHomeworkInternalRequest = new CompleteHomeworkInternalRequestModel
        {
            StudentId = student.Id,
            StudentGroupId = student.GroupId,
            HomeworkId = model.HomeworkId,
            StudentComment = model.StudentComment,
            FilePath = uploadedFile.CloudFileName
        };
        
        var educationResponse = await _educationClient.CompleteHomework(completeHomeworkInternalRequest);
        var completedHomework = educationResponse.Data;
        
        return new WebApiResult(completedHomework);
    }

    /// <summary>
    /// Download file of completed homework
    /// </summary>
    /// 
    /// <remarks>Requires in-header JWT-token with user id, bound to student</remarks>
    ///
    /// <param name="homeworkId">Id of homework</param>
    ///
    /// <response code="200">Success / file_info_not_found / </response>
    /// <response code="401">Unauthorized</response>
    /// <response code="502">hosting_file_not_found</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("homework-solution-file/{homeworkId:int}")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<FileContentResult> GetHomeworkSolutionFile(int homeworkId)
    {
        var getStudentResponse = await _membersClient.GetStudentByUserId(UserId);
        var student = getStudentResponse.Data;

        var getFileResponse = await _filesClient.GetHomeworkSolutionFile(student.Id, homeworkId);
        var file = getFileResponse.Data;
        
        return File(file.FileContent, "application/octet-stream", file.FileName);
    }
    
    /// <summary>
    /// Download file of homework task
    /// </summary>
    /// 
    /// <remarks>Requires in-header JWT-token with user id, bound to student</remarks>
    ///
    /// <param name="homeworkId">Id of homework</param>
    ///
    /// <response code="200">Success / file_info_not_found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="502">hosting_file_not_found</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("homework-task-file/{homeworkId:int}")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<FileContentResult> GetHomeworkTaskFile(int homeworkId)
    {
        var getFileResponse = await _filesClient.GetHomeworkTaskFile(homeworkId);
        var file = getFileResponse.Data;
        
        Response.Headers.Add("Content-Disposition", file.FileName);
        
        return File(file.FileContent, "application/octet-stream", file.FileName);
    }
    
}
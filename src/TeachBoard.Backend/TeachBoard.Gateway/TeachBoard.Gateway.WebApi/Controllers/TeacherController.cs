using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.Gateway.Application;
using TeachBoard.Gateway.Application.Exceptions;
using TeachBoard.Gateway.Application.Refit.Clients;
using TeachBoard.Gateway.Application.Refit.RequestModels.Education;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Education;
using TeachBoard.Gateway.WebApi.ActionResults;
using TeachBoard.Gateway.WebApi.Models;

namespace TeachBoard.Gateway.WebApi.Controllers;

[Route("api/teacher")]
[Authorize(Roles = "Teacher")]
public class TeacherController : BaseController
{
    private readonly IIdentityClient _identityClient;
    private readonly IMembersClient _membersClient;
    private readonly IEducationClient _educationClient;

    private readonly ILogger<TeacherController> _logger;

    public TeacherController(IIdentityClient identityClient, IMembersClient membersClient,
        IEducationClient educationClient, ILogger<TeacherController> logger)
    {
        _identityClient = identityClient;
        _membersClient = membersClient;
        _educationClient = educationClient;
        _logger = logger;
    }

    /// <summary>
    /// Check completed student's homeworks
    /// </summary>
    /// 
    /// <param name="model">Completed homework id, grade and comment</param>
    ///
    /// <response code="200">
    /// Success / teacher_not_found / completed_homework_not_found / completed_homework_invalid_teacher
    /// </response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Invalid model state</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpPost("check-homework")]
    [ProducesResponseType(typeof(CompletedHomework), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<CompletedHomework>> CheckHomework([FromBody] CheckHomeworkRequestModel model)
    {
        var teacherResponse = await _membersClient.GetTeacherByUserId(UserId);
        var teacher = teacherResponse.Data;

        if (teacher is null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.TeacherNotFound,
                PublicErrorMessage = "Teacher data bound to your user not found",
                LogErrorMessage = $"CheckHomework at teacher controller error. No teacher found by user id [{UserId}]"
            };

        var internalRequest = new CheckHomeworkInternalRequestModel
        {
            TeacherId = teacher.Id,
            CompletedHomeworkId = model.CompletedHomeworkId,
            Grade = model.Grade,
            Comment = model.Comment
        };
        var checkHomeworkResponse = await _educationClient.CheckHomework(internalRequest);
        var checkedCompletedHomework = checkHomeworkResponse.Data;

        return new WebApiResult(checkedCompletedHomework);
    }

    /// <summary>
    /// Get teacher's unchecked homeworks
    /// </summary>
    ///
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("unchecked-homeworks")]
    [ProducesResponseType(typeof(IList<CompletedHomework>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IList<CompletedHomework>>> GetUncheckedHomeworks()
    {
        var teacherResponse = await _membersClient.GetTeacherByUserId(UserId);
        var teacher = teacherResponse.Data;

        var getHomeworksResponse = await _educationClient.GetTeacherUncheckedHomeworks(teacher.Id);
        var uncheckedHomeworks = getHomeworksResponse.Data;

        return new WebApiResult(uncheckedHomeworks);
    }

    /// <summary>
    /// Get teacher's future lessons
    /// </summary>
    ///
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("future-lessons")]
    [ProducesResponseType(typeof(IList<Lesson>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IList<Lesson>>> GetFutureLessons()
    {
        var teacherResponse = await _membersClient.GetTeacherByUserId(UserId);
        var teacher = teacherResponse.Data;

        var getLessonsResponse = await _educationClient.GetFutureLessonsByTeacherId(teacher.Id);
        var futureLessonsByTeacher = getLessonsResponse.Data;

        return new WebApiResult(futureLessonsByTeacher);
    }

    /// <summary>
    /// Get full lesson info (lesson + students)
    /// </summary>
    ///
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("lesson-full-info/{lessonId:int}")]
    [ProducesResponseType(typeof(FullLessonInfoResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<FullLessonInfoResponseModel>> GetFullLessonInfo(int lessonId)
    {
        // so bad code

        // get lesson info
        var getLessonResponse = await _educationClient.GetLessonById(lessonId);
        var lesson = getLessonResponse.Data;

        if (lesson is null)
            return new WebApiResult();

        // get students of the lesson
        var groupId = lesson.GroupId;
        var getGroupStudentResponse = await _membersClient.GetStudentsByGroupId(groupId);
        var groupStudents = getGroupStudentResponse.Data;

        // get students ids and user ids
        var studentsUserIds = groupStudents.Select(student => student.UserId).ToList();
        var studentIds = groupStudents.Select(student => student.Id).ToList();

        // get students presentationModels (name, avatar)
        var getStudentsPresentationsResponse = await _identityClient.GetUserPresentationDataModels(studentsUserIds);
        var studentsPresentationModels = getStudentsPresentationsResponse.Data;

        // get students lesson activities
        var getStudentsLessonActivitiesResponse =
            await _educationClient.GetLessonStudentsActivities(lessonId, studentIds);
        var studentsLessonActivities = getStudentsLessonActivitiesResponse.Data;

        // union students presentation data and activity to one model
        var studentPresentationWithActivityModels = from student in groupStudents
            join studentPresentation in studentsPresentationModels on student.UserId equals studentPresentation.Id into
                sp
            from studentPresentation in sp.DefaultIfEmpty()
            join studentActivity in studentsLessonActivities on student.Id equals studentActivity.StudentId into sa
            from studentActivity in sa.DefaultIfEmpty()
            select new StudentPresentationWithActivityModel
            {
                StudentId = student.Id,
                UserId = student.UserId,
                FirstName = studentPresentation?.FirstName,
                LastName = studentPresentation?.LastName,
                Patronymic = studentPresentation?.Patronymic,
                AvatarImagePath = studentPresentation?.AvatarImagePath,
                Grade = studentActivity?.Grade,
                AttendanceStatus = studentActivity?.AttendanceStatus
            };

        var lessonInfo = new FullLessonInfoResponseModel
        {
            Lesson = lesson,
            Students = studentPresentationWithActivityModels
        };

        return new WebApiResult(lessonInfo);
    }
}
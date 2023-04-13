using Microsoft.AspNetCore.Mvc;
using Refit;
using TeachBoard.Gateway.Application;
using TeachBoard.Gateway.Application.Exceptions;
using TeachBoard.Gateway.Application.Refit.Clients;
using TeachBoard.Gateway.Application.Refit.RequestModels.Education;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Education;
using TeachBoard.Gateway.Application.Validation;
using TeachBoard.Gateway.WebApi.ActionResults;
using TeachBoard.Gateway.WebApi.Models;

namespace TeachBoard.Gateway.WebApi.Controllers;

[Route("api/teacher")]
[Microsoft.AspNetCore.Authorization.Authorize(Roles = "Teacher")]
public class TeacherController : BaseController
{
    private readonly IIdentityClient _identityClient;
    private readonly IMembersClient _membersClient;
    private readonly IEducationClient _educationClient;
    private readonly IFilesClient _filesClient;

    private readonly ILogger<TeacherController> _logger;

    public TeacherController(IIdentityClient identityClient, IMembersClient membersClient,
        IEducationClient educationClient, ILogger<TeacherController> logger, IFilesClient filesClient)
    {
        _identityClient = identityClient;
        _membersClient = membersClient;
        _educationClient = educationClient;
        _logger = logger;
        _filesClient = filesClient;
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

    /// <summary>
    /// Check completed student's homeworks
    /// </summary>
    /// 
    /// <param name="model">Completed homework id, grade and comment</param>
    ///
    /// <response code="200">
    /// Success / lesson_not_found / lesson_not_started
    /// </response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Invalid model state</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpPost("student-lesson-activity")]
    [ProducesResponseType(typeof(StudentLessonActivity), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<StudentLessonActivity>> SetStudentLessonActivity(
        [FromBody] SetStudentLessonActivityRequestModel model)
    {
        // todo check student existence
        
        var setActivityResponse = await _educationClient.SetStudentLessonActivity(model);
        var activity = setActivityResponse.Data;

        return new WebApiResult(activity);
    }

    /// <summary>
    /// Create homework for group
    /// </summary>
    /// 
    /// <param name="model">Homework receiver info and file</param>
    ///
    /// <response code="200">
    /// Success / teacher_not_found
    /// </response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Invalid model state</response>
    /// <response code="502">hosting_bad_response</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpPost("homework")]
    [ProducesResponseType(typeof(Homework), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<Homework>> CreateHomework([FromForm] CreateHomeworkRequestModel model)
    {
        var getTeacherResponse = await _membersClient.GetTeacherByUserId(UserId);
        var teacher = getTeacherResponse.Data;
        
        if (teacher is null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.TeacherNotFound,
                PublicErrorMessage = "Teacher data bound to your user not found",
                LogErrorMessage = $"CheckHomework at teacher controller error. No teacher found by user id [{UserId}]"
            };

        var internalRequest = new CreateHomeworkInternalRequestModel
        {
            GroupId = model.GroupId,
            TeacherId = teacher.Id,
            SubjectId = model.SubjectId
        };

        var createHomeworkResponse = await _educationClient.CreateHomework(internalRequest);
        var createdHomework = createHomeworkResponse.Data;
        
        await using var stream = model.File.OpenReadStream();
        var streamPart = new StreamPart(stream, model.File.FileName, "image/jpeg");

        var uploadFileResponse =
            await _filesClient.UploadHomeworkTaskFile(createdHomework.Id, streamPart);

        return new WebApiResult(createdHomework);
    }

    /// <summary>
    /// Set student's examination activity
    /// </summary>
    /// 
    /// <param name="model">Examination id, student id, grade and status</param>
    ///
    /// <response code="200">
    /// Success / student_not_found / examination_not_found
    /// </response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Invalid model state</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpPost("student-examination-activity")]
    [ProducesResponseType(typeof(StudentExaminationActivity), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<StudentExaminationActivity>> SetStudentExaminationActivity(
        [FromBody] SetStudentExaminationActivityRequestModel model)
    {
        var getStudentResponse = await _membersClient.GetStudentById(model.StudentId);
        if (getStudentResponse.Data is null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.StudentNotFound,
                PublicErrorMessage = "Student not found",
                LogErrorMessage =
                    $"SetStudentExaminationActivity at controller error. Student with id [{model.StudentId}] not found"
            };

        var setExamActivityResponse = await _educationClient.SetStudentExaminationActivity(model);
        var examinationActivity = setExamActivityResponse.Data;

        return new WebApiResult(examinationActivity);
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
        
        return File(file.FileContent, "application/octet-stream", file.FileName);
    }
    
    /// <summary>
    /// Download file of completed homework
    /// </summary>
    /// 
    /// <remarks>Requires in-header JWT-token with user id, bound to teacher</remarks>
    ///
    /// <param name="homeworkId">Id of homework</param>
    /// <param name="studentId">Id of student who completed the homework</param>
    ///
    /// <response code="200">Success / file_info_not_found / </response>
    /// <response code="401">Unauthorized</response>
    /// <response code="502">hosting_file_not_found</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("homework-solution-file/{homeworkId:int}/{studentId:int}")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<FileContentResult> GetHomeworkSolutionFile(int homeworkId, int studentId)
    {
        var getFileResponse = await _filesClient.GetHomeworkSolutionFile(studentId, homeworkId);
        var file = getFileResponse.Data;
        
        return File(file.FileContent, "application/octet-stream", file.FileName);
    }
    
    
    /// <summary>
    /// Get current teacher's lesson info (lesson + students)
    /// </summary>
    ///
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("current-lesson")]
    [ProducesResponseType(typeof(FullLessonInfoResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<CurrentLessonResponseModel>> GetCurrentLesson()
    {
        // so bad code

        var teacherResponse = await _membersClient.GetTeacherByUserId(UserId);
        var teacherId = teacherResponse.Data.Id;

        // get lesson info
        var getLessonResponse = await _educationClient.GetTeacherCurrentLesson(teacherId);
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
            await _educationClient.GetLessonStudentsActivities(lesson.Id, studentIds);
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

        var lessonInfo = new CurrentLessonResponseModel
        {
            Lesson = lesson,
            Students = studentPresentationWithActivityModels
        };

        return new WebApiResult(lessonInfo);
    }

    [HttpPut("lesson-topic")]
    public async Task<ActionResult<Lesson>> UpdateLessonTopic([FromBody] UpdateLessonTopicRequestModel model)
    {
        var response = await _educationClient.UpdateLessonTopic(model);
        var updatedLesson = response.Data;

        return new WebApiResult(updatedLesson);
    }
}